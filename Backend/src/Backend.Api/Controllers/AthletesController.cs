using Backend.Infrastructure.CommandQueryHandler;
using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.CommandQueryHandler.Commands.Athletes;
using Backend.Infrastructure.CommandQueryHandler.Queries.Athletes;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Services;
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
    [Route("api/[controller]")]
    [ApiController]
    public class AthletesController : ApiControllerBase
    {
        private readonly ILoggerManager _logger;

        public AthletesController(IDispatcher dispatcher, ILoggerManager logger) 
            : base(dispatcher)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AthleteDto>>> GetAll()
        {
            var athletes = await QueryAsync(new GetAthletes());

            return Ok(athletes);
        }

        [HttpGet("{athleteId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AthleteDto>> Get(int athleteId)
        {
            var athlete = await QueryAsync(new GetAthlete(athleteId));
            if (athlete == null)
            {
                return NotFound();
            }

            return Ok(athlete);
        }

        [HttpGet("{athleteId}/products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AthleteDto>> GetProducts(int athleteId, DateTime? date = null)
        {
            var athlete = await QueryAsync(new GetAthleteProducts(athleteId, date));
            if (athlete == null)
            {
                return NotFound();
            }

            return Ok(athlete);
        }

        [HttpGet("{athleteId}/products/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AthleteDto>> GetProduct(int athleteId, int productId)
        {
            var athlete = await QueryAsync(new GetAthleteProduct(athleteId, productId));
            if (athlete == null)
            {
                return NotFound();
            }

            return Ok(athlete);
        }

        [HttpGet("{athleteId}/exercises")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AthleteDto>> GetExercises(int athleteId, string dayName = "Monday")
        {
            var athlete = await QueryAsync(new GetAthleteExercises(athleteId, dayName));
            if (athlete is null)
            {
                return NotFound();
            }

            return Ok(athlete);
        }

        [HttpGet("{athleteId}/exercises/{exerciseId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AthleteDto>> GetExercise(int athleteId, int exerciseId)
        {
            var athlete = await QueryAsync(new GetAthleteExercise(athleteId, exerciseId));
            if (athlete is null)
            {
                return NotFound();
            }

            return Ok(athlete);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post([FromBody]CreateAthlete command)
        {
            await DispatchAsync(command);

            _logger.LogInfo($"Athlete with user id {command.UserId} added.");

            return CreatedAtAction(nameof(Get), new { athleteId = command.AthleteId }, command);
        
        }

        [HttpDelete("{athleteId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete([FromBody]DeleteAthlete command)
        {
            await DispatchAsync(command);

            _logger.LogInfo($"Athlete with id {command.AthleteId} removed.");

            return NoContent();
        }
    }
}
