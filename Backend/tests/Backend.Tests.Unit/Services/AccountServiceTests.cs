using AutoFixture;
using Backend.Core.Entities;
using Backend.Core.Enums;
using Backend.Core.Exceptions;
using Backend.Core.Factories;
using Backend.Core.Repositories;
using Backend.Infrastructure.Auth;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Extensions;
using Backend.Infrastructure.Services.Account;
using Backend.Infrastructure.Settings;
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

namespace Backend.Tests.Unit.Services
{
    public class AccountServiceTests
    {
        private readonly IPasswordHandler _passwordHandler;
        private readonly IRefreshTokenFactory _refreshTokenFactory;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IJwtHandler _jwtHandler;
        private readonly IUserRepository _userRepository;
        private readonly FakeUserManager _fakeUserManager;
        private readonly IEmailService _emailService;
        private readonly AccountService _sut;
        private readonly IFixture _fixture;
        private readonly GeneralSettings _generalSettings;

        public AccountServiceTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _passwordHandler = Substitute.For<IPasswordHandler>();
            _refreshTokenFactory = Substitute.For<IRefreshTokenFactory>();
            _refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
            _userRepository = Substitute.For<IUserRepository>();
            _jwtHandler = Substitute.For<IJwtHandler>();

            _fakeUserManager = Substitute.For<FakeUserManager>();
            _emailService = Substitute.For<IEmailService>();
            _generalSettings = Substitute.For<GeneralSettings>();

            _sut = new AccountService(_userRepository, _passwordHandler, _jwtHandler, _refreshTokenFactory,
                _refreshTokenRepository, _fakeUserManager, _generalSettings, _emailService);
        }

        [Theory]
        [InlineData("testUser", "test@mail.com", "testSecret")]
        public async Task SignUpAsync_ShouldAddNewUser(string username, string email, string password)
        {
            // Arrange 
            var user = _fixture.Build<User>()
                .With(u => u.UserName, username)
                .With(u => u.Email, email)
                .Create();

            var token = _fixture.Create<string>();

            _fakeUserManager.CreateAsync(Arg.Any<User>(), Arg.Any<string>()).Returns(IdentityResult.Success);

            _fakeUserManager.GenerateEmailConfirmationTokenAsync(Arg.Any<User>()).Returns(token);

            // Act
            var id = await _sut.SignUpAsync(username, email, password);

            // Assert
            await _fakeUserManager.Received(1).CreateAsync(Arg.Is<User>(u => 
            u.UserName == username &&
            u.Email == email), Arg.Is(password));
        }
         
        [Theory]
        [InlineData("testUser", "test@mail.com", "testSecret")]
        public async Task SignUp_ShouldThrowException_WhenUserNameIsAlreadyTaken(string userName, string email,
            string password)
        {
            var user = _fixture.Build<User>()
            .With(u => u.UserName, userName)
            .With(u => u.Email, email)
            .Create();

            await _fakeUserManager.CreateAsync(user, password);

            _fakeUserManager.CreateAsync(Arg.Any<User>(), Arg.Any<string>()).Returns(IdentityResult.Failed(new IdentityError
            {
                Description = $"Username '{userName}' already taken."
            }));

            var exception = await Record.ExceptionAsync(() => _sut.SignUpAsync(userName, email, password));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(IdentityResultException));
        }

        [Fact]
        public async Task SignInAsync_ShouldReturnJwtDto_WhenDataIsValid()
        {
            var user = _fixture.Build<User>()
                .With(u => u.EmailConfirmed, true)
                .Create();
            _userRepository.GetByUsernameAsync(user.UserName).Returns(user);

            _passwordHandler.IsValid(user.PasswordHash, "password").ReturnsForAnyArgs(true);

            _fakeUserManager.Options.SignIn.RequireConfirmedEmail = true;

            var jwtDto = _fixture.Create<JwtDto>();

            _jwtHandler.CreateToken(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>()).Returns(jwtDto);

            var refreshToken = new RefreshToken();

            _refreshTokenFactory.Create(Arg.Any<int>()).Returns(refreshToken);

            var jwt = await _sut.SignInAsync(user.UserName, user.PasswordHash);

            jwt.ShouldNotBeNull();
            jwt.ShouldBeOfType(typeof(JwtDto));
            jwt.UserId.ShouldBe(jwtDto.UserId);
            jwt.Username.ShouldBe(jwtDto.Username);
            jwt.Role.ShouldBe(jwtDto.Role);
            jwt.AccessToken.ShouldBe(jwtDto.AccessToken);
            jwt.Expires.ShouldBe(jwtDto.Expires);
            jwt.RefreshToken.ShouldBe(jwtDto.RefreshToken);
        }

        [Fact]
        public async Task SignInAsync_ShouldException_WhenUserDoesNotExist()
        {
            // Arrange
            var username = "test";
            var password = "testPassword";

            // Act
            var exception = await Record.ExceptionAsync(() => _sut.SignInAsync(username, password));

            // Assert
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(InvalidCredentialsException));
            exception.Message.ShouldBe("Invalid credentials.");
        }

        [Fact]
        public async Task SignInAsync_ShouldException_WhenPasswordIsNotValid()
        {
            // Arrange
            var user = _fixture.Create<User>();
            _userRepository.GetByUsernameAsync(user.UserName).Returns(user);
            _passwordHandler.IsValid(Arg.Any<string>(), Arg.Any<string>()).Returns(false);

            // Act
            var exception = await Record.ExceptionAsync(() => _sut.SignInAsync(user.UserName, user.PasswordHash));

            // Arrange
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(InvalidCredentialsException));
            exception.Message.ShouldBe("Invalid credentials.");
        }

        [Fact]
        public async Task SignInAsync_ShouldThrowException_WhenEmailIsNotConfirmed()
        {
            var user = _fixture.Create<User>();
            _userRepository.GetByUsernameAsync(user.UserName).Returns(user);
            _passwordHandler.IsValid(Arg.Any<string>(), Arg.Any<string>()).ReturnsForAnyArgs(true);
            _fakeUserManager.Options.SignIn.RequireConfirmedEmail = true;

            // Act
            var exception = await Record.ExceptionAsync(() => _sut.SignInAsync(user.UserName, user.PasswordHash));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(EmailNotConfirmedException));
            exception.Message.ShouldBe("Email not confirmed. Confirm email to get access.");
        }

        [Fact]
        public async Task ChangePasswordAsync_ShouldReturnIdentityResultSuccess_WhenUserExistsAndCurrentPasswordIsValid()
        {
            var oldPassword = "oldpassword";
            var newPassword = "newPassword";

            var user = _fixture.Create<User>();

            _userRepository.GetOrFailAsync(Arg.Any<int>()).Returns(user);

            _fakeUserManager.ChangePasswordAsync(user, oldPassword, newPassword).Returns(IdentityResult.Success);

            var result = await _sut.ChangePasswordAsync(user.Id, oldPassword, newPassword);

            result.ShouldBe(IdentityResult.Success);
        }

        [Fact]
        public async Task ChangePasswordAsync_ShouldThrowException_WhenUserDoesNotExist()
        {
            var userId = 5;
            var oldPassword = "oldpassword";
            var newPassword = "newPassword";

            var exception = await Record.ExceptionAsync(() => _sut.ChangePasswordAsync(userId, oldPassword,
                newPassword));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(UserNotFoundException));
            exception.Message.ShouldBe($"User with id: '{userId}' was not found.");
        }

        [Fact]
        public async Task ChangePasswordAsync_ShouldThrowException_WheNUserExistsButCurrentPasswordIsNotValid()
        {
            var user = _fixture.Create<User>();
            var oldPassword = "oldpassword";
            var newPassword = "newPassword";

            _userRepository.GetOrFailAsync(Arg.Any<int>()).Returns(user);

            _fakeUserManager.ChangePasswordAsync(Arg.Any<User>(), Arg.Any<string>(), Arg.Any<string>()).Returns(IdentityResult.Failed(
                new IdentityError
                {
                    Description = "Current password is not valid."
                }));

            var exception = await Record.ExceptionAsync(() => _sut.ChangePasswordAsync(user.Id, oldPassword,
                newPassword));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(IdentityResultException));
            exception.Message.ShouldBe("Current password is not valid.");
        }

        //[Fact]
        //public async Task GenerateEmailConfirmationTokenAsync_ShouldInvokeSendConfirmationAsync_WhenTokenIsValid()
        //{
        //    var user = _fixture.Build<User>()
        //        .With(u => u.EmailConfirmed, true)
        //        .Create();

        //    await _fakeUserManager.CreateAsync(user);
        //    var token = _fixture.Create<string>();

        //    _fakeUserManager.GenerateEmailConfirmationTokenAsync(user).Returns(token);

        //    await _sut.GenerateEmailConfirmationTokenAsync(user);
        //}

        [Fact]
        public async Task GenerateEmailConfirmationTokenAsync_ShouldThrowException_WhenTokenIsNullOrEmpty()
        {
            var user = _fixture.Create<User>();

            var exception = await Record.ExceptionAsync(() => _sut.GenerateEmailConfirmationTokenAsync(user));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(InvalidEmailTokenException));
            exception.Message.ShouldBe("Invalid email confirmation token.");
        }
    }
}
