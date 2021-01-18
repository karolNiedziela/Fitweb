using Backend.Infrastructure.CommandHandler.Commands;
using Backend.Infrastructure.CommandHandler.Commands.External;
using Backend.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Api.Controllers
{
    [ApiController]
    [Route("api/")]
    public class ExternalLoginController : ApiControllerBase
    {
        private readonly IMemoryCache _cache;

        public ExternalLoginController(ICommandDispatcher commandDispatcher, IMemoryCache cache)
            : base(commandDispatcher)
        {
            _cache = cache;
        }

        [HttpPost]
        [Route("facebook")]
        public async Task<IActionResult> Post([FromBody]FacebookLogin command)
        {
            command.TokenId = Guid.NewGuid();
            await DispatchAsync(command);
            var jwt = _cache.GetJwt(command.TokenId);

            return Ok(jwt);
        }
    }
}
