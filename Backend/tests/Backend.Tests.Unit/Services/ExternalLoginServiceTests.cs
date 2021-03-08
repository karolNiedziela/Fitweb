using Backend.Core.Entities;
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private readonly IExternalLoginService _sut;

        public ExternalLoginServiceTests()
        {
            _facebookAuthService = Substitute.For<IFacebookAuthService>();
            _userRepository = Substitute.For<IUserRepository>();
            _jwtHandler = Substitute.For<IJwtHandler>();
            _refreshTokenFactory = Substitute.For<IRefreshTokenFactory>();
            _refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
            _sut = new ExternaLoginService(_facebookAuthService, _userRepository, _jwtHandler, 
                _refreshTokenFactory, _refreshTokenRepository);
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

            var user = new User
            {
                Id = 10,
                UserName = facebookInfoResult.Email,
                Email = facebookInfoResult.Email,
                UserRoles = new List<UserRole>
                {
                    new UserRole
                    {
                        UserId = 10,
                        RoleId = Role.GetRole(RoleId.User.ToString()).Id
                    }
                }
            };
            var jwtDto = new JwtDto
            {
                UserId = user.Id,
                Username = user.UserName,
                Role = Role.GetRole("User").Name,
                AccessToken = "1234.56712.12323",
                Expires = 10000000,
                RefreshToken = string.Empty
            };

            _jwtHandler.CreateToken(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>()).Returns(jwtDto);
            var refreshToken = new RefreshToken();

            _refreshTokenFactory.Create(Arg.Any<int>()).Returns(refreshToken);

            var jwt = await _sut.LoginWithFacebookAsync(accessToken);

            jwt.ShouldNotBeNull();
            jwt.UserId.ShouldBe(jwtDto.UserId);
            jwt.Username.ShouldBe(jwtDto.Username);
            jwt.Role.ShouldBe(jwtDto.Role);
            jwt.AccessToken.ShouldBe(jwtDto.AccessToken);
            jwt.Expires.ShouldBe(jwtDto.Expires);
            jwt.RefreshToken.ShouldBe(jwtDto.RefreshToken);
            jwt.ShouldBeOfType(typeof(JwtDto));
            await _userRepository.Received(1).AddAsync(Arg.Any<User>());
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
            exception.ShouldBeOfType(typeof(ServiceException));
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

            var jwtDto = new JwtDto
            {
                UserId = user.Id,
                Username = user.UserName,
                Role = Role.GetRole("User").Name,
                AccessToken = "1234.56712.12323",
                Expires = 10000000,
                RefreshToken = string.Empty
            };

            _jwtHandler.CreateToken(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>()).Returns(jwtDto);
            var refreshToken = new RefreshToken();

            _refreshTokenFactory.Create(Arg.Any<int>()).Returns(refreshToken);

            var jwt = await _sut.LoginWithFacebookAsync(accessToken);

            jwt.ShouldNotBeNull();
            jwt.UserId.ShouldBe(jwtDto.UserId);
            jwt.Username.ShouldBe(jwtDto.Username);
            jwt.Role.ShouldBe(jwtDto.Role);
            jwt.AccessToken.ShouldBe(jwtDto.AccessToken);
            jwt.Expires.ShouldBe(jwtDto.Expires);
            jwt.RefreshToken.ShouldBe(jwtDto.RefreshToken);
            jwt.ShouldBeOfType(typeof(JwtDto));
            await _userRepository.Received(0).AddAsync(Arg.Is(user));
        }
    }
}
