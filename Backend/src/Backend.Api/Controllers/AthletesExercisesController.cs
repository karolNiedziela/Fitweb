using Backend.Infrastructure.CommandQueryHandler;
using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.CommandQueryHandler.Queries.Athletes;
using Backend.Infrastructure.DTO;
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
    [Route("athlete/exercises")]
    [ApiController]
    public class AthletesExercisesController : ApiControllerBase
    {
        private readonly ILoggerManager _logger;

        public AthletesExercisesController(IDispatcher dispatcher, ILoggerManager logger)
            : base(dispatcher)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AthleteDto>> GetAll(string dayName = "Monday")
        {
            var athlete = await QueryAsync(new GetAthleteExercises(UserId, dayName));
            if (athlete is null)
            {
                return NotFound();
            }

            return Ok(athlete);
        }

        [HttpGet("{exerciseId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AthleteDto>> Get(int exerciseId)
        {
            var athlete = await QueryAsync(new GetAthleteExercise(UserId, exerciseId));
            if (athlete is null)
            {
                return NotFound();
            }

            return Ok(athlete);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post([FromBody]AddAthleteExercise command)
        {
            await DispatchAsync(command);

            _logger.LogInfo($"Exercise with id: {command.ExerciseId} added to athlete with user id: {command.UserId}.");

            return CreatedAtAction(nameof(Get),
                new { exerciseId = command.ExerciseId }, command);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete([FromBody]DeleteAthleteExercise command)
        {
            await DispatchAsync(command);

            _logger.LogInfo($"Exercise with id: {command.ExerciseId} added to athlete with user id: {command.UserId}.");

            return NoContent();
        }
    }
}
