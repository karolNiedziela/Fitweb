using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.CommandQueryHandler;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Backend.Infrastructure.Auth;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ApiControllerBase
    {
        private readonly IRefreshTokenService _refreshTokenService;

        public TokensController(IDispatcher dispatcher, IRefreshTokenService refreshTokenService) 
            : base(dispatcher)
        {
            _refreshTokenService = refreshTokenService;
        }

        [HttpPost("use")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<JwtDto>> Use(string refreshToken)
            => await _refreshTokenService.UseAsync(refreshToken);


        [HttpPost("revoke")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Revoke(string refreshtoken)
        {
            await _refreshTokenService.RevokeAsync(refreshtoken);

            return NoContent();
        }
    }
}
