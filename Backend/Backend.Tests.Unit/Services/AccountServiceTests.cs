using Backend.Core.Entities;
using Backend.Core.Exceptions;
using Backend.Core.Factories;
using Backend.Core.Repositories;
using Backend.Infrastructure.Auth;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Services.Account;
using Backend.Tests.Unit.Fixtures;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Unit.Services
{
    public class AccountServiceTests : IClassFixture<FitwebFixture>
    {
        private readonly IPasswordHandler _passwordHandler;
        private readonly IRefreshTokenFactory _refreshTokenFactory;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IJwtHandler _jwtHandler;
        private readonly IUserRepository _userRepository;
        private readonly IAccountService _sut;
        private readonly FitwebFixture _fixture;

        public AccountServiceTests(FitwebFixture fixture)
        {
            _fixture = fixture;
            _passwordHandler = Substitute.For<IPasswordHandler>();
            _refreshTokenFactory = Substitute.For<IRefreshTokenFactory>();
            _refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
            _userRepository = Substitute.For<IUserRepository>();
            _jwtHandler = Substitute.For<IJwtHandler>();
            _sut = new AccountService(_userRepository, _passwordHandler, _jwtHandler, _refreshTokenFactory,
                _refreshTokenRepository);
        }

        [Theory]
        [InlineData("testUser", "test@mail.com", "testSecret")]
        public async Task SignUpAsync_ShouldAddNewUser(string username, string email, string password)
        {
            // Arrange 
            _userRepository.AnyAsync(Arg.Any<Expression<Func<User, bool>>>()).Returns(false);

            // Act
            await _sut.SignUpAsync(username, email, password);

            // Assert
            await _userRepository.Received(1).AddAsync(Arg.Any<User>());
        }

        [Theory]
        [InlineData("user1", "user1@email.com", "secret")]
        [InlineData("user2", "user2@email.com", "secret")]
        [InlineData("user3", "user3@email.com", "secret")]
        public async Task SignUpAsync_ShouldThrowException_WhenUserNameIsNotUnique(string username,
            string email, string password)
        {
            var user = _fixture.FitwebContext.Users.SingleOrDefault(u => u.Username == username);
            _userRepository.GetByUsernameAsync(username).Returns(user);

            var exception = await Record.ExceptionAsync(() =>
                _sut.SignUpAsync(username, email, password));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.Message.ShouldBe($"Username is already taken.");
        }

        [Theory]
        [InlineData("us", "user1@email.com", "secret")]
        [InlineData("us1", "user2@email.com", "secret")]
        public async Task SignUpAsync_ShouldThrowException_WhenUserNameIsTooShort(string username, string email,
            string password)
        {
            var exception = await Record.ExceptionAsync(() => _sut.SignUpAsync(username, email, password));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(DomainException));
            ((DomainException)exception).Code.ShouldBe(Core.Exceptions.ErrorCodes.InvalidUsername);
            exception.Message.ShouldBe("Username must contain at least 6 characters " +
                    "and at most twenty characters.");
        }

        [Theory]
        [InlineData("user1", "user1@email.com", "secret")]
        [InlineData("user2", "user2@email.com", "secret")]
        [InlineData("user3", "user3@email.com", "secret")]
        public async Task SignUpAsync_ShouldThrowException_WhenEmailIsNotUnique(string username,
            string email, string password)
        {
            // Arranges
            var user = _fixture.FitwebContext.Users.SingleOrDefault(u => u.Email == email);
            _userRepository.GetByEmailAsync(email).Returns(user);

            // Act
            var exception = await Record.ExceptionAsync(() =>
                _sut.SignUpAsync(username, email, password));

            // Assert
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.Message.ShouldBe($"Email is already taken.");
        }

        [Theory]
        [InlineData("user1", "user1mail.com", "secret")]
        [InlineData("user2", "user2mail.com", "secret")]
        public async Task SignUpAsync_ShouldThrowException_WhenEmailIsNotValid(string username, string email, string password)
        {
            _userRepository.AnyAsync(Arg.Any<Expression<Func<User, bool>>>()).Returns(false);

            var exception = await Record.ExceptionAsync(() => _sut.SignUpAsync(username, email, password));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(DomainException));
            ((DomainException)exception).Code.ShouldBe(Core.Exceptions.ErrorCodes.InvalidEmail);
            exception.Message.ShouldBe("Invalid email format.");
        }

        [Fact]
        public async Task SignInAsync_ShouldReturnJwtDto_WhenDataIsValid()
        {
            var user = _fixture.FitwebContext.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).SingleOrDefault(u => u.Id == 1);
            _userRepository.GetByUsernameAsync(user.Username).Returns(user);

            _passwordHandler.IsValid(user.Password, "password").ReturnsForAnyArgs(true);

            var jwtDto = new JwtDto
            {
                UserId = user.Id,
                Username = user.Username,
                Role = user.UserRoles.Select(ur => ur.Role.Name.ToString()).FirstOrDefault(),
                AccessToken = "1234.56712.12323",
                Expires = 10000000,
                RefreshToken = string.Empty
            };

            _jwtHandler.CreateToken(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>()).Returns(jwtDto);

            var refreshToken = new RefreshToken();

            _refreshTokenFactory.Create(Arg.Any<int>()).Returns(refreshToken);

            var jwt = await _sut.SignInAsync(user.Username, user.Password);

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
            exception.StackTrace.Length.Equals(2);
        }

        [Fact]
        public async Task LoginAsync_ShouldThrowException_WhenPasswordIsNotValid()
        {
            // Arrange
            var user = _fixture.FitwebContext.Users.SingleOrDefault(u => u.Id == 1);
            _userRepository.GetAsync(user.Id).Returns(user);
            _passwordHandler.IsValid(Arg.Is(user.Password), Arg.Any<string>()).Returns(false);

            // Act
            var exception = await Record.ExceptionAsync(() => _sut.SignInAsync(user.Username, user.Password));

            // Arrange
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.Message.ShouldBe("Invalid credentials");
        }

    }
}
