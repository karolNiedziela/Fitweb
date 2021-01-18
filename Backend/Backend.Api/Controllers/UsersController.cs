using Backend.Infrastructure.CommandHandler.Commands;
using Backend.Infrastructure.CommandHandler.Commands.Users;
using Backend.Infrastructure.Services;
using Backend.Infrastructure.Services.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Backend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ApiControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILoggerManager _logger;

        public UsersController(ICommandDispatcher commandDispatcher, IUserService userService, ILoggerManager logger)
            : base(commandDispatcher)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _userService.GetAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet]
        //GET : /api/users
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();

            return Ok(users);
        }

        [HttpPost]
        //POST : /api/users
        public async Task<IActionResult> Post([FromBody]CreateUser command)
        {
            await DispatchAsync(command);

            _logger.LogInfo($"User with username {command.Username} and added.");

            return CreatedAtAction(nameof(Get), new { id = command.Id }, command );
        }

        //DELETE : /api/users/{id}
        [HttpDelete]
        //[Authorize(Roles ="Admin")]
        public async Task<IActionResult> Delete([FromBody] DeleteUser command)
        {
            await _userService.DeleteAsync(command.Id);

            _logger.LogInfo($"User with user id: {command.Id} was removed.");

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]UpdateUser command)
        {
            await DispatchAsync(command);

            _logger.LogInfo($"User with user id: {command.Id} updated profile data.");

            return CreatedAtAction(nameof(Get), new { id = command.Id }, command);
        }
    }
}
