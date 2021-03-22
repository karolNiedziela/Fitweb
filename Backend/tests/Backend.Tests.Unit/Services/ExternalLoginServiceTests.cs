using AutoFixture;
using Backend.Core.Entities;
using Backend.Core.Enums;
using Backend.Core.Factories;
using Backend.Core.Repositories;
using Backend.Infrastructure.Auth;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.External;
using Backend.Infrastructure.Services.External;
using Backend.Tests.Unit.Fixtures;
using NSubstitute;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Unit.Services
{
    public class ExternalLoginServiceTests
    {
        private readonly IFacebookAuthService _facebookAuthService;
        private readonly IUserRepository _userRepository;
        private readonly IJwtHandler _jwtHandler;
        private readonly IRefreshTokenFactory _refreshTokenFactory;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ExternaLoginService _sut;
        private readonly FakeUserManager _fakeUserManager;
        private readonly IFixture _fixture;

        public ExternalLoginServiceTests()
        {
            _fixture = new Fixture();
            _facebookAuthService = Substitute.For<IFacebookAuthService>();
            _userRepository = Substitute.For<IUserRepository>();
            _jwtHandler = Substitute.For<IJwtHandler>();
            _refreshTokenFactory = Substitute.For<IRefreshTokenFactory>();
            _refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
            _fakeUserManager = Substitute.For<FakeUserManager>();
            _sut = new ExternaLoginService(_facebookAuthService, _userRepository, _jwtHandler,
                _refreshTokenFactory, _refreshTokenRepository, _fakeUserManager);
        }

        [Fact]
        public async Task LoginWithFacebookAsync_ShouldAddNewUser_IfUserDoesNotExistAndTokenIsValid()
        {
            var accessToken = "EAABw3KiLV1QBACrZCNuvHBaijiPEURQzAhVqZCG";

            var facebookTokenValidationResult = new FacebookTokenValidationResult
            {
                FacebookTokenValidationData = new FacebookTokenValidationData
                {
                    IsValid = true
                }
            };

            var facebookInfoResult = new FacebookUserInfoResult
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "test",
                LastName = "user",
                Email = "testUser@email.com",

            };

            _facebookAuthService.ValidateAccessTokenAsync(accessToken).Returns(facebookTokenValidationResult);

            _facebookAuthService.GetUserInfoAsync(accessToken).Returns(facebookInfoResult);

            var jwtDto = _fixture.Create<JwtDto>();

            _jwtHandler.CreateToken(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>()).Returns(jwtDto);
            var refreshToken = _fixture.Create<RefreshToken>();

            _refreshTokenFactory.Create(Arg.Any<int>()).Returns(refreshToken);

            var jwt = await _sut.LoginWithFacebookAsync(accessToken);

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
        public async Task LoginWithFacebookAsync_ShouldThrowException_WhenTokenIsNotValid()
        {
            var accessToken = "EAABw3KiLV1QBACrZCNuvHBaijiPEURQzAhVqZCG";

            var facebookTokenValidationResult = new FacebookTokenValidationResult
            {
                FacebookTokenValidationData = new FacebookTokenValidationData
                {
                    IsValid = false
                }
            };

            _facebookAuthService.ValidateAccessTokenAsync(accessToken).Returns(facebookTokenValidationResult);

            var exception = await Record.ExceptionAsync(() => _sut.LoginWithFacebookAsync(accessToken));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(InvalidFacebookTokenException));
            exception.Message.ShouldBe("Invalid facebook token.");
        }

        [Fact]
        public async Task LoginWithFacebookAsync_ShouldSignIn_WhenUserExists()
        {
            var accessToken = "EAABw3KiLV1QBACrZCNuvHBaijiPEURQzAhVqZCG";

            var facebookTokenValidationResult = new FacebookTokenValidationResult
            {
                FacebookTokenValidationData = new FacebookTokenValidationData
                {
                    IsValid = true
                }
            };

            var facebookInfoResult = new FacebookUserInfoResult
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "test",
                LastName = "user",
                Email = "testuser@email.com",

            };

            _facebookAuthService.ValidateAccessTokenAsync(accessToken).Returns(facebookTokenValidationResult);

            _facebookAuthService.GetUserInfoAsync(accessToken).Returns(facebookInfoResult);

            var user = new User(facebookInfoResult.Email, facebookInfoResult.Email);

            _userRepository.GetByEmailAsync(facebookInfoResult.Email).Returns(user);

            var jwtDto = _fixture.Create<JwtDto>();

            _jwtHandler.CreateToken(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>()).Returns(jwtDto);
            var refreshToken = _fixture.Create<RefreshToken>();

            _refreshTokenFactory.Create(Arg.Any<int>()).Returns(refreshToken);

            var jwt = await _sut.LoginWithFacebookAsync(accessToken);

            jwt.ShouldNotBeNull();
            jwt.ShouldBeOfType(typeof(JwtDto));
            jwt.UserId.ShouldBe(jwtDto.UserId);
            jwt.Username.ShouldBe(jwtDto.Username);
            jwt.Role.ShouldBe(jwtDto.Role);
            jwt.AccessToken.ShouldBe(jwtDto.AccessToken);
            jwt.Expires.ShouldBe(jwtDto.Expires);
            jwt.RefreshToken.ShouldBe(jwtDto.RefreshToken);
        }
    }
}
