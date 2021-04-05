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
using Backend.Infrastructure.CommandQueryHandler.Commands.Tokens;
using Microsoft.Extensions.Caching.Memory;
using Backend.Infrastructure.Extensions;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ApiControllerBase
    {
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IMemoryCache _cache;

        public TokensController(IDispatcher dispatcher, IRefreshTokenService refreshTokenService,
            IMemoryCache cache) 
            : base(dispatcher)
        {
            _refreshTokenService = refreshTokenService;
            _cache = cache;
        }

        [HttpPost("use")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<JwtDto>> Use([FromBody] UseToken command)
        {
            command.TokenId = new Guid();
            await DispatchAsync(command);
            var jwt = _cache.GetJwt(command.TokenId);
            return Ok(jwt);
        }


        [HttpPost("revoke")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Revoke([FromBody] RevokeToken command)
        {
            await DispatchAsync(command);

            return NoContent();
        }
    }
}
