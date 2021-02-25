using Backend.Core.Entities;
using Backend.Core.Repositories;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.External;
using Backend.Infrastructure.Services.External;
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
        private readonly IExternalLoginService _sut;

        public ExternalLoginServiceTests()
        {
            _facebookAuthService = Substitute.For<IFacebookAuthService>();
            _userRepository = Substitute.For<IUserRepository>();
            _sut = new ExternaLoginService(_facebookAuthService, _userRepository);
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

            var email = await _sut.LoginWithFacebookAsync(accessToken);

            email.ShouldBe(facebookInfoResult.Email);
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

            var email = await _sut.LoginWithFacebookAsync(accessToken);

            email.ShouldBe(user.Email);
            await _userRepository.Received(0).AddAsync(Arg.Is(user));
        }
    }
}
