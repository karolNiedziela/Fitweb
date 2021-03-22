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
        private readonly JwtHandler _sut;

        public JwtHandlerTests()
        {
            _settings = Substitute.For<JwtSettings>();
            _settings.IssuerSigningKey = "some_random_key_123";
            _settings.ExpiryMinutes = 20;
            _settings.Issuer = "fitweb";
            _settings.Algorithm = "HS256";
            _sut = Substitute.For<JwtHandler>(_settings);
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
            var jwtDto = _sut.CreateToken(userId, username, role);

            jwtDto.ShouldNotBeNull();
            jwtDto.UserId.ShouldBe(userId);
            jwtDto.Username.ShouldBe(username);
            jwtDto.Role.ShouldBe(role);
            jwtDto.Expires.ShouldBeGreaterThanOrEqualTo(expires.ToUnixTimeMilliseconds());
            jwtDto.RefreshToken.ShouldBeEmpty();
            jwtDto.AccessToken.ShouldNotBeNull();
        }
    }
}