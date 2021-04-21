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
using Backend.Infrastructure.Services;
using Backend.Infrastructure.Services.Account;
using Backend.Infrastructure.Settings;
using Backend.Tests.Unit.Fixtures;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
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
        private readonly IAthleteService _athleteService;

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
            _athleteService = Substitute.For<IAthleteService>();

            _sut = new AccountService(_userRepository, _passwordHandler, _jwtHandler, _refreshTokenFactory,
                _refreshTokenRepository, _fakeUserManager, _generalSettings, _emailService, _athleteService);
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

            _fakeUserManager.CreateAsync(user, password).ReturnsForAnyArgs(IdentityResult.Success);

            _fakeUserManager.GenerateEmailConfirmationTokenAsync(user).ReturnsForAnyArgs(token);

            // Act
            await _sut.SignUpAsync(username, email, password);

            // Assert
            await _fakeUserManager.Received(1).CreateAsync(Arg.Is<User>(u => 
            u.UserName == username &&
            u.Email == email), Arg.Is(password));
        }

        [Fact]
        public async Task SignUp_ShouldThrowException_WhenUsernameIsEmpty()
        {
            var username = "";
            var email = "test@email.com";
            var role = "role";

            var exception = await Record.ExceptionAsync(() => _sut.SignUpAsync(username, email, 
               role));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(EmptyUsernameException));
            exception.Message.ShouldBe("Username cannot be empty.");
        }

        [Theory]
        [InlineData(3)]
        [InlineData(45)]
        public async Task SignUp_ShouldThrowException_WhenUsernameLengthIsInvalid(int textLength)
        {
            var username = new string('a', textLength);
            var email = "test@email.com";
            var password = "password";

            var exception = await Record.ExceptionAsync(() => _sut.SignUpAsync(username, email, password));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(InvalidUsernameException));
            exception.Message.ShouldBe("Username must contain at least 4 characters and at most 40 characters.");
        }

        [Fact]
        public async Task SignUp_ShouldThrowException_WhenEmailIsEmpty()
        {
            var username = "testUsername";
            var email = "";
            var password = "passsword";

            var exception = await Record.ExceptionAsync(() => _sut.SignUpAsync(username, email,
               password));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(EmptyEmailException));
            exception.Message.ShouldBe("Email cannot be empty.");
        }

        [Fact]
        public async Task SignUp_ShouldThrowException_WhenEmailFormatIsInvalid()
        {
            var username = "testUsername";
            var email = "testEmail";
            var password = "passsword";

            var exception = await Record.ExceptionAsync(() => _sut.SignUpAsync(username, email, password));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(InvalidEmailException));
            exception.Message.ShouldBe("Invalid email format.");
        }

        [Fact]
        public async Task SignUp_ShouldThrowException_WhenPasswordIsEmpty()
        {
            var username = "testUsername";
            var email = "test@email.com";
            var password = "";

            var exception = await Record.ExceptionAsync(() => _sut.SignUpAsync(username, email, password));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(EmptyPasswordException));
            exception.Message.ShouldBe("Password cannot be empty.");
        }

        [Theory]
        [InlineData(5)]
        [InlineData(25)]
        public async Task SignUp_ShouldThrowException_WhenPasswordLenghtIsInvalid(int passLength)
        {
            var username = "testUsername";
            var email = "test@email.com";
            var password = _fixture.Create<string>().Substring(0, length: passLength);

            var exception = await Record.ExceptionAsync(() => _sut.SignUpAsync(username, email, password));

            exception.ShouldBeOfType(typeof(InvalidPasswordException));
            exception.Message.ShouldBe("Password cannot contain less than 6 characters and more than 20 " +
                "characters.");
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

            _fakeUserManager.CreateAsync(user, password).ReturnsForAnyArgs(IdentityResult.Failed(new IdentityError
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

            _jwtHandler.CreateToken(user.Id, user.UserName, user.UserRoles.FirstOrDefault().Role.Name.ToString())
                .Returns(jwtDto);

            var refreshToken = new RefreshToken();

            _refreshTokenFactory.Create(user.Id).Returns(refreshToken);

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
            var password = "notMatching";
            var user = _fixture.Create<User>();
            _userRepository.GetByUsernameAsync(user.UserName).Returns(user);
            _passwordHandler.IsValid(user.PasswordHash, password).Returns(false);

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
            var password = "notMatching";
            var user = _fixture.Create<User>();
            _userRepository.GetByUsernameAsync(user.UserName).Returns(user);
            _passwordHandler.IsValid(user.PasswordHash, password).ReturnsForAnyArgs(true);
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

            _fakeUserManager.FindByIdAsync(user.Id.ToString()).Returns(user);

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

            _fakeUserManager.FindByIdAsync(user.Id.ToString()).Returns(user);

            _fakeUserManager.ChangePasswordAsync(user, oldPassword, newPassword).Returns(IdentityResult.Failed(
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

        [Fact]
        public async Task GenerateEmailConfirmationTokenAsync_ShouldThrowException_WhenTokenIsNullOrEmpty()
        {
            var user = _fixture.Create<User>();

            var exception = await Record.ExceptionAsync(() => _sut.GenerateEmailConfirmationTokenAsync(user));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(InvalidEmailTokenException));
            exception.Message.ShouldBe("Invalid email confirmation token.");
        }

        [Fact]
        public async Task ConfirmEmailAsync_ShouldReturnIdentityResultSuccess_WhenEmailWasConfirmed()
        {
            var user = _fixture.Create<User>();
            var code = _fixture.Create<string>();

            _fakeUserManager.FindByIdAsync(user.Id.ToString()).Returns(user);

            _fakeUserManager.ConfirmEmailAsync(user, code).ReturnsForAnyArgs(IdentityResult.Success);

            var result = await _sut.ConfirmEmailAsync(user.Id, code);

            result.ShouldBe(IdentityResult.Success);
        }

        [Fact]
        public async Task ConfirmEmailAsync_ShouldThrowException_WhenUserDoesNotExist()
        {
            var userId = 5;
            var code = _fixture.Create<string>();

            var exception = await Record.ExceptionAsync(() => _sut.ConfirmEmailAsync(userId, code));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(UserNotFoundException));
            exception.Message.ShouldBe($"User with id: '{userId}' was not found.");
        }

        [Fact]
        public async Task ResetPasswordAsync_ShouldReturnIdentityResultSuccess_WhenPasswordWasReseted()
        {
            var user = _fixture.Create<User>();
            var code = _fixture.Create<string>();
            var newPassword = "newPassword";

            _fakeUserManager.FindByIdAsync(user.Id.ToString()).Returns(user);

            _fakeUserManager.ResetPasswordAsync(user, code, newPassword)
                .ReturnsForAnyArgs(IdentityResult.Success);

            var result = await _sut.ResetPasswordAsync(user.Id, code, newPassword);

            result.ShouldBe(IdentityResult.Success);
        }

        [Fact]
        public async Task ResetPassword_ShouldThrowException_WhenUserDoesNotExist()
        {
            var userId = 2;
            var code = _fixture.Create<string>();
            var newPassword = "newPassword";

            var exception = await Record.ExceptionAsync(() => _sut.ResetPasswordAsync(userId, code, newPassword));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(UserNotFoundException));
            exception.Message.ShouldBe($"User with id: '{userId}' was not found.");
        }

        [Fact]
        public async Task GenerateForgotPasswordTokenAsync_ShouldThrowException_WhenUseruserExistsButTokenIsEmpty()
        {
            var email = "test@email.com";
            var user = _fixture.Build<User>()
                .With(u => u.Email, email)
                .Create();

            _fakeUserManager.FindByEmailAsync(email).Returns(user);

            var exception = await Record.ExceptionAsync(() => _sut.GenerateForgotPasswordTokenAsync(email));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(InvalidForgotPasswordTokenException));
            exception.Message.ShouldBe("Invalid forgot password token.");
        }
    }
}
