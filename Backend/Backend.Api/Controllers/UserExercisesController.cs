using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Backend.Infrastructure.Commands;
using Backend.Infrastructure.Commands.UserExercises;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.EF;
using Backend.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserExercisesController : ApiControllerBase
    {
        private readonly IUserExerciseService _userExerciseService;

        public UserExercisesController(ICommandDispatcher commandDispatcher, IUserExerciseService userExerciseService) : base(commandDispatcher)
        {
            _userExerciseService = userExerciseService;
        }

        //GET: /api/userexercises
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userProducts = await _userExerciseService.GetAllAsync();

            return Json(userProducts);
        }

        //GET : /api/userexercises/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllExercises(int id)
        {
            var userExercises = await _userExerciseService.GetAllExercisesForUserAsync(id);

            return Json(userExercises);
        }

        //POST : /api/userexercises
        [HttpPost]
        public async Task<IActionResult> Post(AddUserExercise command)
        {
            await DispatchAsync(command);

            return Ok();
        }

        //PUT : /api/userexercises
        [HttpPut]
        public async Task<IActionResult> Put(UpdateUserExercise command)
        {
            await DispatchAsync(command);

            return NoContent();
        }


        //DELETE : /api/userexercises/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userExerciseService.DeleteAsync(id);

            return NoContent();
        }
    }
}
