using Backend.Infrastructure.CommandHandler.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Api.Controllers
{
    [Route("athletes/exercises")]
    [ApiController]
    public class AthletesExercisesController : ApiControllerBase
    {
        public AthletesExercisesController(ICommandDispatcher commandDispatcher) : base(commandDispatcher)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddAthleteExercise command)
        {
            await DispatchAsync(command);

            return CreatedAtAction("GetExercise", "Athletes",
                new { userId = command.UserId, exerciseId = command.ExerciseId }, command);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteAthleteExercise command)
        {
            await DispatchAsync(command);

            return NoContent();
        }
    }
}
