using Backend.Infrastructure.Commands;
using Backend.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserDietGoalsController : ApiControllerBase
    {
        private readonly IDietGoalsService _dietGoalsService;

        public UserDietGoalsController(ICommandDispatcher commandDispatcher, IDietGoalsService dietGoalsService) 
            : base(commandDispatcher)
        {
            _dietGoalsService = dietGoalsService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userDietGoal = await _dietGoalsService.GetAsync(UserId);

            return Json(userDietGoal);
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddDietGoals command)
        {
            command.UserId = UserId;
            await DispatchAsync(command);

            return Ok();
        }
    }
}
