using Backend.Infrastructure.Commands;
using Backend.Infrastructure.Commands.Users;
using Backend.Infrastructure.EF;
using Backend.Infrastructure.Extensions;
using Backend.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ApiControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMemoryCache _cache;

        public UsersController(IUserService userService, IMemoryCache cache,
            ICommandDispatcher commandDispatcher)
            : base(commandDispatcher)
        {
            _userService = userService;
            _cache = cache;
        }

        [HttpGet("{username}")]
        //GET : /api/users/{username}
        public async Task<IActionResult> Get(string username)
        {
            var user = await _userService.GetAsync(username);
            if (user == null)
            {
                return NotFound();
            }

            return Json(user);
        }

        [HttpGet]
        //GET : /api/users
        public async Task<IActionResult> Get()
        {
            var users = await _userService.GetAllAsync();

            return Json(users);
        }

        [HttpPost]
        [Route("register")]
        //POST : /api/users/register
        public async Task<IActionResult> Register([FromBody] CreateUser command)
        {
            await DispatchAsync(command);

            return Ok(new { code = "succeeded" });
        }

        [HttpPost]
        [Route("login")]
        //POST : /api/users/login
        public async Task<IActionResult> Login([FromBody] Login command)
        {
            command.TokenId = Guid.NewGuid();
            await DispatchAsync(command);
            var jwt = _cache.GetJwt(command.TokenId);

            return Json(jwt);

        }

        //DELETE : /api/users/{id}
        [HttpDelete("{userId}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Delete(int userId)
        {
            await _userService.DeleteAsync(userId);

            return NoContent();
        }
    }
}
