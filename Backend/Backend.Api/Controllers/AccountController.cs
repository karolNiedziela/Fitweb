using Backend.Infrastructure.Commands;
using Backend.Infrastructure.Commands.Account;
using Backend.Infrastructure.Handlers.Account;
using Backend.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class AccountController : ApiControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserCalorieCounter _userCaloriesCounter;
        private readonly IDietGoalsService _dietGoalsService;

        public AccountController(ICommandDispatcher commandDispatcher, IUserService userService, IUserCalorieCounter userCalorieCounter,
            IDietGoalsService dietGoalsService) 
            : base(commandDispatcher)
        {
            _userService = userService;
            _userCaloriesCounter = userCalorieCounter;
            _dietGoalsService = dietGoalsService;

        }

        [HttpGet]

        //GET : /api/account
        public async Task<Object> GetUserAccount()
        { 
            var user = await _userService.GetAsync(UserId);

            return Json(user);
        }

        [HttpGet]
        [Route("Calories")]
        //GET : /api/account/calories
        public async Task<IActionResult> GetCalories()
        {
            var result = await _userCaloriesCounter.CountCalories(UserId);
            return Json(result);
        }

        [HttpGet]
        [Route("DietGoals")]
        //GET : /api/account/dietgoals
        public async Task<IActionResult> Get()
        {
         
            var userDietGoal = await _dietGoalsService.GetAsync(UserId);

            return Json(userDietGoal);
        }

        //POST : /api/account/changepassword
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePassword command)
        {
            await DispatchAsync(command);

            return Ok();
        }

        //POST : /api/account/editprofile
        [HttpPost]
        [Route("EditProfile")]
        public async Task<IActionResult> EditProfile(EditProfile command)
        {
            await DispatchAsync(command);

            return Ok();
        }

        //POST : /api/account/dietGoals
        [HttpPost]
        [Route("DietGoals")]
        public async Task<IActionResult> AddDietGoals(AddDietGoals command)
        {
            await DispatchAsync(command);

            return Ok();
        }

    }
}
