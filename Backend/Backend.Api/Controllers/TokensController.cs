using Backend.Infrastructure.CommandHandler.Commands;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ApiControllerBase
    {
        private readonly IRefreshTokenService _refreshTokenService;

        public TokensController(ICommandDispatcher commandDispatcher, IRefreshTokenService refreshTokenService) 
            : base(commandDispatcher)
        {
            _refreshTokenService = refreshTokenService;
        }

        [HttpPost("use")]
        public async Task<ActionResult<JwtDto>> Use(string refreshToken)
            => await _refreshTokenService.UseAsync(refreshToken);


        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke(string refreshToken)
        {
            await _refreshTokenService.RevokeAsync(refreshToken);
            return NoContent();
        }
    }
}
