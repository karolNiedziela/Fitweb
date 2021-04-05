using Backend.Infrastructure.CommandQueryHandler.Commands.Tokens;
using Backend.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers.Tokens
{
    public class RevokeTokenHandler : ICommandHandler<RevokeToken>
    {
        private readonly IRefreshTokenService _refreshTokenService;

        public RevokeTokenHandler(IRefreshTokenService refreshTokenService)
        {
            _refreshTokenService = refreshTokenService;
        }

        public async Task HandleAsync(RevokeToken command)
        {
            await _refreshTokenService.RevokeAsync(command.RefreshToken);
        }
    }
}
