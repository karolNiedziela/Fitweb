using Backend.Infrastructure.Auth;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Extensions;
using Backend.Infrastructure.Settings;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;
using Shouldly;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace Backend.Tests.Unit.Auth
{
    public class JwtHandlerTests
    {
        private readonly JwtSettings _settings;
        private readonly IJwtHandler _jwtHandler;

        public JwtHandlerTests()
        {
            _settings = Substitute.For<JwtSettings>();
            _settings.Key = "some_random_key_123";
            _settings.ExpiryMinutes = 10;
            _settings.Issuer = "https://localhost:5001/";
            _jwtHandler = Substitute.For<JwtHandler>(_settings);
        }

        [Fact]
        public void CreateToken_ShouldReturnNewJwtDto()
        {
            var userId = 10;
            var username = "user10";
            var role = "User";
            var now = DateTime.UtcNow;
            var expires = now.AddMinutes(_settings.ExpiryMinutes);

            // Assert
            var jwtDto = _jwtHandler.CreateToken(userId, username, role);

            jwtDto.ShouldNotBeNull();
            jwtDto.UserId.ShouldBe(userId);
            jwtDto.Username.ShouldBe(username);
            jwtDto.Role.ShouldBe(role);
            jwtDto.Expires.ShouldBeGreaterThanOrEqualTo(expires.ToTimeStamp());
            jwtDto.RefreshToken.ShouldBeEmpty();
            jwtDto.AccessToken.ShouldNotBeNull();
        }
    }
}