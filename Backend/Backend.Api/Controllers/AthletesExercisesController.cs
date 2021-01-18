using Backend.Infrastructure.CommandHandler.Commands;
using Backend.Infrastructure.Services.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILoggerManager _logger;

        public AthletesExercisesController(ICommandDispatcher commandDispatcher, ILoggerManager logger) 
            : base(commandDispatcher)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddAthleteExercise command)
        {
            await DispatchAsync(command);

            _logger.LogInfo($"Exercise with id: {command.ExerciseId} added to user with id: {command.UserId}.");

            return CreatedAtAction("GetExercise", "Athletes",
                new { userId = command.UserId, exerciseId = command.ExerciseId }, command);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteAthleteExercise command)
        {
            await DispatchAsync(command);

            _logger.LogInfo($"Exercise with id: {command.ExerciseId} added to user with id: {command.UserId}.");

            return NoContent();
        }
    }
}
