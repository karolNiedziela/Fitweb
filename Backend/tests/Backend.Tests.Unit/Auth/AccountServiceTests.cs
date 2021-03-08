using Backend.Core.Entities;
using Backend.Core.Exceptions;
using Backend.Core.Factories;
using Backend.Core.Repositories;
using Backend.Infrastructure.Auth;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Services.Account;
using Backend.Tests.Unit.Fixtures;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Unit.Auth
{
    public class AccountServiceTests : IClassFixture<FitwebFixture>
    {
        private readonly IPasswordHandler _passwordHandler;
        private readonly IRefreshTokenFactory _refreshTokenFactory;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IJwtHandler _jwtHandler;
        private readonly IUserRepository _userRepository;
        private readonly FakeUserManager _fakeUserManager;
        private readonly IEmailService _emailService;
        private readonly IAccountService _sut;
        private readonly FitwebFixture _fixture;
        private readonly IConfiguration _configuration;

        public AccountServiceTests(FitwebFixture fixture)
        {
            _fixture = fixture;
            _passwordHandler = Substitute.For<IPasswordHandler>();
            _refreshTokenFactory = Substitute.For<IRefreshTokenFactory>();
            _refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
            _userRepository = Substitute.For<IUserRepository>();
            _jwtHandler = Substitute.For<IJwtHandler>();

            _fakeUserManager = Substitute.For<FakeUserManager>();
            _emailService = Substitute.For<IEmailService>();
            _configuration = Substitute.For<IConfiguration>();

            _sut = new AccountService(_userRepository, _passwordHandler, _jwtHandler, _refreshTokenFactory,
                _refreshTokenRepository, _fakeUserManager, _configuration, _emailService);
        }

        [Theory]
        [InlineData("testUser", "test@mail.com", "testSecret")]
        public async Task SignUpAsync_ShouldAddNewUser(string username, string email, string password)
        {
            // Arrange 

            var role = _fixture.FitwebContext.Roles.SingleOrDefault(r => r.Name == Role.GetRole("User").Name);

            var randomString = new RandomStringGenerator(20);

            _fakeUserManager.CreateAsync(Arg.Any<User>(), Arg.Any<string>()).Returns(IdentityResult.Success);

            _fakeUserManager.GenerateEmailConfirmationTokenAsync(Arg.Any<User>()).Returns(randomString.RandomString);

            // Act
            var id = await _sut.SignUpAsync(username, email, password);
        }

        [Fact]
        public async Task SignInAsync_ShouldReturnJwtDto_WhenDataIsValid()
        {
            var user = _fixture.FitwebContext.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).SingleOrDefault(u => u.Id == 1);
            _userRepository.GetByUsernameAsync(user.UserName).Returns(user);

            user.EmailConfirmed = true;

            _passwordHandler.IsValid(user.PasswordHash, "password").ReturnsForAnyArgs(true);

            var jwtDto = new JwtDto
            {
                UserId = user.Id,
                Username = user.UserName,
                Role = user.UserRoles.Select(ur => ur.Role.Name.ToString()).FirstOrDefault(),
                AccessToken = "1234.56712.12323",
                Expires = 10000000,
                RefreshToken = string.Empty
            };

            _jwtHandler.CreateToken(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>()).Returns(jwtDto);

            var refreshToken = new RefreshToken();

            _refreshTokenFactory.Create(Arg.Any<int>()).Returns(refreshToken);

            var jwt = await _sut.SignInAsync(user.UserName, user.PasswordHash);

            jwt.ShouldNotBeNull();
            jwt.UserId.ShouldBe(jwtDto.UserId);
            jwt.Username.ShouldBe(jwtDto.Username);
            jwt.Role.ShouldBe(jwtDto.Role);
            jwt.AccessToken.ShouldBe(jwtDto.AccessToken);
            jwt.Expires.ShouldBe(jwtDto.Expires);
            jwt.RefreshToken.ShouldBe(jwtDto.RefreshToken);
            jwt.ShouldBeOfType(typeof(JwtDto));
        }

        [Fact]
        public async Task SignInAsync_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange

            // Act
            var exception = await Record.ExceptionAsync(() => _sut.SignInAsync("karol", "testSecret"));

            // Assert
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.Message.ShouldBe("Invalid credentials");
        }

        [Fact]
        public async Task SignInAsync_ShouldThrowException_WhenPasswordIsNotValid()
        {
            // Arrange
            var user = _fixture.FitwebContext.Users.SingleOrDefault(u => u.Id == 1);
            _userRepository.GetByUsernameAsync(user.UserName).Returns(user);
            _passwordHandler.IsValid(Arg.Is(user.PasswordHash), Arg.Any<string>()).Returns(false);

            // Act
            var exception = await Record.ExceptionAsync(() => _sut.SignInAsync(user.UserName, user.PasswordHash));

            // Arrange
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.Message.ShouldBe("Invalid credentials");
        }

        [Fact]
        public async Task SignInAsync_ShouldThrowError_WhenEmailIsNotConfirmed()
        {
            var user = _fixture.FitwebContext.Users.SingleOrDefault(u => u.Id == 1);
            _userRepository.GetByUsernameAsync(user.UserName).Returns(user);
            _passwordHandler.IsValid(user.PasswordHash, "password").ReturnsForAnyArgs(true);
            _fakeUserManager.Options.SignIn.RequireConfirmedEmail = true;

            // Act
            var exception = await Record.ExceptionAsync(() => _sut.SignInAsync(user.UserName, user.PasswordHash));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.Message.ShouldBe("Email not confirmed. Confirm email to get access.");
            ((ServiceException)exception).Code.ShouldBe(Infrastructure.Exceptions.ErrorCodes.EmailNotConfirmed);
        }

        [Fact]
        public async Task GenerateEmailConfirmationTokenAsync_ShouldInvokeSendConfirmationAsync_WhenTokenIsValid()
        {
            var user = _fixture.FitwebContext.Users.SingleOrDefault(u => u.Id == 1);
            user.EmailConfirmed = true;
            var randomStringGenerator = new RandomStringGenerator(20);

            await _fakeUserManager.CreateAsync(user);
            var token = randomStringGenerator.RandomString;

            _fakeUserManager.GenerateEmailConfirmationTokenAsync(user).Returns(token);

            await _sut.GenerateEmailConfirmationTokenAsync(user);
        }

        [Fact]
        public async Task GenerateEmailConfirmationTokenAsync_ShouldThrowException_WhenTokenIsNullOrEmpty()
        {
            var user = new User();

            var exception = await Record.ExceptionAsync(() => _sut.GenerateEmailConfirmationTokenAsync(user));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.Message.ShouldBe("Invalid email confirmation token.");
            ((ServiceException)exception).Code.ShouldBe(Infrastructure.Exceptions.ErrorCodes.InvalidToken);
        }
    }
}
