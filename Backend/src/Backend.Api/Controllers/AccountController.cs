using Backend.Infrastructure.CommandQueryHandler;
using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.CommandQueryHandler.Queries;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Extensions;
using Backend.Infrastructure.Services.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace Backend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ApiControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly ILoggerManager _logger;

        public AccountController(IDispatcher dispatcher, IMemoryCache cache,
            ILoggerManager logger) : base(dispatcher)
        {
            _cache = cache;
            _logger = logger;
        }

        [HttpGet]
        [Route("me")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserDto>> Get()
        {
            var user = await QueryAsync(new Me(UserId));

            return Ok(user);
        }

        [HttpPost]
        [Route("signin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<JwtDto>> SignInAsync([FromBody]SignIn command)
        {
            command.TokenId = Guid.NewGuid();
            await DispatchAsync(command);
            var jwt = _cache.GetJwt(command.TokenId);
            return Ok(jwt);
        }

        [HttpPost]
        [Route("signup")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //POST : /api/account/signup
        public async Task<IActionResult> SignUp([FromBody] SignUp command)
        {
            await DispatchAsync(command);

            _logger.LogInfo($"User with username {command.Username}.");

            return CreatedAtAction("Get", "Users", new { id = command.Id }, command);
        }

        [HttpPatch]
        [Route("changepassword")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> ChangePasswordAsync([FromBody]ChangePassword command)
        {
            command.UserId = UserId;
            await DispatchAsync(command);

            return NoContent();
        }

        [HttpPost]
        [Route("forgotpassword")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgotPassword([FromBody]ForgotPassword command) 
        {
            await DispatchAsync(command);

            return NoContent();
        }
    }
}
