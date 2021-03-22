using AutoFixture;
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
    public class RefreshTokenServiceTests
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IRefreshTokenFactory _refreshTokenFactory;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IRng _rng;
        private readonly IJwtHandler _jwtHandler;
        private readonly IUserService _userService;
        private readonly RefreshTokenService _sut;
        private readonly JwtSettings _settings;
        private readonly IFixture _fixture;

        public RefreshTokenServiceTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
               .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _dateTimeProvider = Substitute.For<IDateTimeProvider>();
            _rng = Substitute.For<Rng>();
            _refreshTokenFactory = Substitute.For<RefreshTokenFactory>(_rng, _dateTimeProvider);
            _refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
            _settings = Substitute.For<JwtSettings>();
            _settings.IssuerSigningKey = "some_random_key_123";
            _settings.ExpiryMinutes = 10;
            _settings.Issuer = "fitweb";
            _settings.Algorithm = "HS256";
            _jwtHandler = Substitute.For<JwtHandler>(_settings);
            _userService = Substitute.For<IUserService>();
            _sut = new RefreshTokenService(_refreshTokenRepository, _userService, _refreshTokenFactory, 
                _dateTimeProvider, _jwtHandler);

        }

        [Theory]
        [InlineData(10, "testUser1", "User")]
        [InlineData(20, "testUser2", "User")]
        public async Task UseAsync_ShouldReturnJwtDto_WhenRefreshTokenExists(int id, string userName, string role)
        {
            var token = _fixture.Build<RefreshToken>()
                .With(t => t.UserId, id) 
                .Create();
            _refreshTokenRepository.GetAsync(Arg.Any<string>()).Returns(token);
            var user = _fixture.Build<UserDto>()
                .With(u => u.Id, id)
                .With(u => u.UserName, userName)
                .With(u => u.Role, role)
                .Create();
            _userService.GetAsync(id).Returns(user);

            var jwt = await _sut.UseAsync(token.Token);

            jwt.ShouldNotBeNull();
            jwt.UserId.ShouldBe(user.Id);
            jwt.Username.ShouldBe(user.UserName);
            jwt.Role.ShouldBe(user.Role);
            await _refreshTokenRepository.Received(1).GetAsync(token.Token);
            await _userService.Received(1).GetAsync(id);
            _jwtHandler.Received(1).CreateToken(token.UserId, user.UserName, user.Role);
            _refreshTokenFactory.Received(1).Create(id);
            await _refreshTokenRepository.Received(1).AddAsync(Arg.Any<RefreshToken>());
            await _refreshTokenRepository.Received(1).UpdateAsync(token);
        }


        [Fact]
        public async Task UseAsync_ShouldThrowException_WhenRefreshTokenIsNull()
        {
            var refreshToken = "notExistingToken";

            var exception = await Record.ExceptionAsync(() => _sut.UseAsync(refreshToken));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(InvalidRefreshTokenException));
            exception.Message.ShouldBe("Invalid refresh token.");
        }

        [Fact]
        public async Task RevokeAsync_ShouldRevokeToken_WhenRefreshTokenExists()
        {
            var refreshToken = _fixture.Create<RefreshToken>();
            
            _refreshTokenRepository.GetAsync(Arg.Any<string>()).Returns(refreshToken);

            await _sut.RevokeAsync(refreshToken.Token);

            refreshToken.Revoked.ShouldBeTrue();
            await _refreshTokenRepository.Received(1).UpdateAsync(refreshToken);
        }


        [Fact]
        public async Task RevokeAsync_ShouldThrowException_WhenRefreshTokenIsNull()
        {
            var refreshToken = "notExistingToken";

            var exception = await Record.ExceptionAsync(() => _sut.RevokeAsync(refreshToken));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(InvalidRefreshTokenException));
            exception.Message.ShouldBe("Invalid refresh token.");
        }
    }
}
