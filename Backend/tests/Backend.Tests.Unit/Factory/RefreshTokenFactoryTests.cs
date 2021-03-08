using Backend.Core.Factories;
using Backend.Core.Services;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Unit.Factory
{
    public class RefreshTokenFactoryTests
    {
        private readonly IRng _rng;

        private readonly IDateTimeProvider _dateTimeProvider;

        private readonly IRefreshTokenFactory _refreshTokenFactory;

        public RefreshTokenFactoryTests()
        {
            _rng = Substitute.For<Rng>();
            _dateTimeProvider = Substitute.For<DateTimeProvider>();
            _refreshTokenFactory = Substitute.For<RefreshTokenFactory>(_rng, _dateTimeProvider);
        }

        [Fact]
        public void Create_ShouldReturnNewRefreshToken()
        {
            var userId = 10;

            var refreshToken = _refreshTokenFactory.Create(userId);

            refreshToken.ShouldNotBeNull();
            refreshToken.Revoked.ShouldBeFalse();
            refreshToken.Token.Length.ShouldBeGreaterThanOrEqualTo(30);
            refreshToken.UserId.ShouldBe(10);
            refreshToken.RevokedAt.ShouldBeNull();
        }
    }
}
