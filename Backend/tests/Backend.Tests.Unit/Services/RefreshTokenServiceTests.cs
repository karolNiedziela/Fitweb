using AutoFixture;
using AutoMapper;
using Backend.Core.Entities;
using Backend.Core.Exceptions;
using Backend.Core.Factories;
using Backend.Core.Repositories;
using Backend.Core.Services;
using Backend.Infrastructure.Auth;
using Backend.Infrastructure.DTO;
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
        private readonly IJwtHandler _jwtHandler;
        private readonly IUserService _userService;
        private readonly RefreshTokenService _sut;
        private readonly IFixture _fixture;

        public RefreshTokenServiceTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
               .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _dateTimeProvider = Substitute.For<IDateTimeProvider>();
            _refreshTokenFactory = Substitute.For<RefreshTokenFactory>(new Rng(), _dateTimeProvider);
            _refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
            _jwtHandler = Substitute.For<IJwtHandler>();
            _userService = Substitute.For<IUserService>();
            _sut = new RefreshTokenService(_refreshTokenRepository, _userService, _refreshTokenFactory, 
                _dateTimeProvider, _jwtHandler);

        }

        [Theory]
        [InlineData(10, "testUser1", "User")]
        [InlineData(20, "testUser2", "User")]
        public async Task UseAsync_ShouldReturnJwtDto_WhenRefreshTokenExists(int id, string userName, string role)
        {

            var token = "randomTestToken";
            var refreshToken = _fixture.Build<RefreshToken>()
                .With(t => t.UserId, id)            
                .Create();

            var user = _fixture.Build<UserDto>()
                .With(u => u.Id, id)
                .With(u => u.UserName, userName)
                .With(u => u.Role, role)
                .Create();

            var jwtDto = _fixture.Build<JwtDto>()
                .With(j => j.UserId, id)
                .With(j => j.Username, userName)
                .With(j => j.Role, role)
                .Create();

            _refreshTokenRepository.GetAsync(token).Returns(refreshToken);
            
            _userService.GetAsync(id).Returns(user);

            _jwtHandler.CreateToken(id, userName, role).Returns(jwtDto);

            var jwt = await _sut.UseAsync(token);

            jwt.ShouldNotBeNull();
            jwt.UserId.ShouldBe(id);
            jwt.Username.ShouldBe(userName);
            jwt.Role.ShouldBe(role);
            await _refreshTokenRepository.Received(1).AddAsync(Arg.Any<RefreshToken>());
            await _refreshTokenRepository.Received(1).UpdateAsync(Arg.Any<RefreshToken>());
        }


        [Fact]
        public async Task UseAsync_ShouldThrowException_WhenRefreshTokenIsNull()
        {
            var refreshToken = "notExistingToken";

            var exception = await Record.ExceptionAsync(() => _sut.UseAsync(refreshToken));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(Infrastructure.Exceptions.InvalidRefreshTokenException));
            exception.Message.ShouldBe("Invalid refresh token.");
        }

        [Fact]
        public async Task UseAsync_ShouldThrowException_WhenRefreshTokenIsRevoked()
        {
            var refreshToken = _fixture.Build<RefreshToken>()
                .Create();
            var token = "randomTestToken";
            
            _refreshTokenRepository.GetAsync(token).Returns(refreshToken);

            refreshToken.Revoke(DateTime.Now);

            var exception = await Record.ExceptionAsync(() => _sut.UseAsync(token));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(Core.Exceptions.RevokedRefreshTokenException));
            exception.Message.ShouldBe("Revoked refresh token.");
        }

        [Fact]
        public async Task RevokeAsync_ShouldRevokeToken_WhenRefreshTokenExists()
        {
            var refreshToken = _fixture.Create<RefreshToken>();
            var token = "randomTestToken";
            
            _refreshTokenRepository.GetAsync(token).Returns(refreshToken);

            await _sut.RevokeAsync(token);

            refreshToken.Revoked.ShouldBeTrue();
            await _refreshTokenRepository.Received(1).UpdateAsync(refreshToken);
        }

        [Fact]
        public async Task RevokeAsync_ShouldThrowException_WhenTokenIsAlreadyRevoked()
        {
            var refreshToken = _fixture.Build<RefreshToken>()
                .Create();
            var token = "randomTestToken";

            refreshToken.Revoke(DateTime.Now);

            _refreshTokenRepository.GetAsync(token).Returns(refreshToken);

            var exception = await Record.ExceptionAsync(() => _sut.RevokeAsync(token));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(RevokedRefreshTokenException));
            exception.Message.ShouldBe("Revoked refresh token.");
        }

        [Fact]
        public async Task RevokeAsync_ShouldThrowException_WhenRefreshTokenIsNull()
        {
            var refreshToken = "notExistingToken";

            var exception = await Record.ExceptionAsync(() => _sut.RevokeAsync(refreshToken));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(Infrastructure.Exceptions.InvalidRefreshTokenException));
            exception.Message.ShouldBe("Invalid refresh token.");
        }
    }
}
