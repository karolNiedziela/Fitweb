using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Infrastructure.Commands;
using Backend.Infrastructure.Commands.Exercises;
using Backend.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExercisesController : ApiControllerBase
    {
        private readonly IExerciseService _exerciseService;

        public ExercisesController(ICommandDispatcher commandDispatcher, IExerciseService exerciseService) 
            : base(commandDispatcher)
        {
            _exerciseService = exerciseService;
        }

        [HttpGet("{name}")]
        //GET : /api/exercises/name
        public async Task<IActionResult> Get(string name)
        {
            var exercise = await _exerciseService.GetAsync(name);
            if (exercise == null)
            {
                return NotFound();
            }

            return Json(exercise);
        }

        [HttpGet]
        //GET : /api/exercises
        public async Task<IActionResult> Get()
        {
            var exercises = await _exerciseService.GetAllAsync();
            if (exercises == null)
            {
                return NotFound();
            }

            return Json(exercises);
        }

        //PUT : /api/exercises
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(UpdateExercise command)
        {
            await DispatchAsync(command);

            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        //POST : /api/exercises
        public async Task<IActionResult> Post(AddExercise command)
        {
            await DispatchAsync(command);

            return Created($"/api/exercises/{command.Name}", null);
        }

        //DELETE : /api/exercises/{id}
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _exerciseService.DeleteAsync(id);

            return NoContent();
        }
    }
}
