using AutoMapper;
using Backend.Core.Entities;
using Backend.Core.Factories;
using Backend.Core.Repositories;
using Backend.Core.Services;
using Backend.Infrastructure.Auth;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Mappers;
using Backend.Infrastructure.Repositories;
using Backend.Infrastructure.Services;
using Backend.Infrastructure.Settings;
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
    public class RefreshTokenServiceTests : IClassFixture<FitwebFixture>
    {
        private readonly IRefreshTokenRepository _repository;
        private readonly IRefreshTokenFactory _refreshTokenFactory;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IRng _rng;
        private readonly IJwtHandler _jwtHandler;
        private readonly IUserService _userService;
        private readonly IRefreshTokenService _sut;
        private readonly JwtSettings _settings;
        private readonly FitwebFixture _fixture;

        public RefreshTokenServiceTests(FitwebFixture fixture)
        {
            _fixture = fixture;
            _dateTimeProvider = Substitute.For<IDateTimeProvider>();
            _rng = Substitute.For<Rng>();
            _refreshTokenFactory = Substitute.For<RefreshTokenFactory>(_rng, _dateTimeProvider);
            _repository = Substitute.For<IRefreshTokenRepository>();
            _settings = Substitute.For<JwtSettings>();
            _settings.IssuerSigningKey = "some_random_key_123";
            _settings.ExpiryMinutes = 10;
            _settings.Issuer = "fitweb";
            _settings.Algorithm = "HS256";
            _jwtHandler = Substitute.For<JwtHandler>(_settings);
            _userService = Substitute.For<IUserService>();
            _sut = new RefreshTokenService(_repository, _userService, _refreshTokenFactory, _dateTimeProvider, _jwtHandler);

        }

        [Fact]
        public async Task UseAsync_ShouldReturnJwtDto_WhenRefreshTokenExists()
        {
            _repository.GetAsync("randomTestToken").Returns(_fixture.FitwebContext.RefreshTokens
                                                    .SingleOrDefault(rt => rt.Token == "randomTestToken"));
            var user = _fixture.FitwebContext.Users.SingleOrDefault(u => u.Id == 3);
            _userService.GetAsync(3).Returns(new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = "User"
            });

            var jwt = await _sut.UseAsync("randomTestToken");

            jwt.ShouldNotBeNull();
            jwt.UserId.ShouldBe(3);
            jwt.Username.ShouldBe("user3");
            jwt.Role.ShouldBe("User");
            await _repository.Received(1).GetAsync(Arg.Any<string>());
            await _userService.Received(1).GetAsync(Arg.Any<int>());
            _jwtHandler.Received(1).CreateToken(3, "user3", "User");
            _refreshTokenFactory.Received(1).Create(1);
            await _repository.Received(1).AddAsync(Arg.Any<RefreshToken>());
            await _repository.Received(1).UpdateAsync(Arg.Any<RefreshToken>());
        }


        [Fact]
        public async Task UseAsync_ShouldThrowException_WhenRefreshTokenIsNull()
        {
            var refreshToken = "sadsadadsassasa";

            var exception = await Record.ExceptionAsync(() => _sut.UseAsync(refreshToken));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.Message.ShouldBe("Invalid refresh token.");
        }

        [Fact]
        public async Task RevokeAsync_ShouldRevokeToken_WhenRefreshTokenExists()
        {
            _repository.GetAsync("randomTestToken2").Returns(_fixture.FitwebContext.RefreshTokens
                                                    .SingleOrDefault(rt => rt.Token == "randomTestToken2"));

            await _sut.RevokeAsync("randomTestToken2");

            var token = await _repository.GetAsync("randomTestToken2");
            token.Revoked.ShouldBeTrue();
            await _repository.Received(1).UpdateAsync(Arg.Any<RefreshToken>());
        }


        [Fact]
        public async Task RevokeAsync_ShouldThrowException_WhenRefreshTokenIsNull()
        {
            var refreshToken = "sadsadadsassasaasdsadsad";

            var exception = await Record.ExceptionAsync(() => _sut.RevokeAsync(refreshToken));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.Message.ShouldBe("Invalid refresh token.");
        }
    }
}
