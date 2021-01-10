using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Infrastructure.CommandHandler.Commands;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var exercise = await _exerciseService.GetAsync(id);
            if (exercise is null)
            {
                return NotFound();
            }

            return Ok(exercise);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var exercises = await _exerciseService.GetAllAsync();

            return Ok(exercises);
        }


        [HttpPost]
      //  [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody]AddExercise command)
        {
            await DispatchAsync(command);

            return CreatedAtAction(nameof(Get), new { id = command.Id }, command);
        }

        [HttpDelete]
     //   [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _exerciseService.DeleteAsync(id);

            return NoContent();
        }

        [HttpPut]
    //  [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put([FromBody]UpdateExercise command)
        {
            await DispatchAsync(command);

            return CreatedAtAction(nameof(Get), new { id = command.Id }, command);
        }
    }
}
