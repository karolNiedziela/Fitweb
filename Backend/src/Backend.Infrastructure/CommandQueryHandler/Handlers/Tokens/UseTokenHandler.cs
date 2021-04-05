using Backend.Infrastructure.CommandQueryHandler.Commands.Tokens;
using Backend.Infrastructure.Extensions;
using Backend.Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers.Tokens
{
    public class UseTokenHandler : ICommandHandler<UseToken>
    {
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IMemoryCache _cache;

        public UseTokenHandler(IRefreshTokenService refreshTokenService, IMemoryCache cache)
        {
            _refreshTokenService = refreshTokenService;
            _cache = cache;
        }

        public async Task HandleAsync(UseToken command)
        {
            var jwt = await _refreshTokenService.UseAsync(command.RefreshToken);
            _cache.SetJwt(command.TokenId, jwt);

        }
    }
}
