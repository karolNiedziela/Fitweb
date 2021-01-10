using Backend.Core.Entities;
using Backend.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Core.Factories
{
    public class RefreshTokenFactory : IRefreshTokenFactory
    {
        private readonly IRngService _rng;

        private readonly IDateTimeProvider _dateTimeProvider;

        public RefreshTokenFactory(IRngService rng, IDateTimeProvider dateTimeProvider)
        {
            _rng = rng;
            _dateTimeProvider = dateTimeProvider;
        }

        public RefreshToken Create(int userId)
            => new RefreshToken(userId, _rng.Generate(), _dateTimeProvider.Now);
    }
}
