using Backend.Infrastructure.CommandQueryHandler;
using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.CommandQueryHandler.Commands.Athletes;
using Backend.Infrastructure.CommandQueryHandler.Queries.Athletes;
using Backend.Infrastructure.Services;
using Backend.Infrastructure.Services.Logger;
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
        public async Task<IActionResult> GetAll()
        {
            var athletes = await QueryAsync(new GetAthletes());

            return Ok(athletes);
        }

        [HttpGet("{athleteId}")]
        public async Task<IActionResult> Get(int athleteId)
        {
            var athlete = await QueryAsync(new GetAthlete(athleteId));
            if (athlete == null)
            {
                return NotFound();
            }

            return Ok(athlete);
        }

        [HttpGet("{athleteId}/products")]
        public async Task<IActionResult> GetProducts(int athleteId, DateTime? date = null)
        {
            var athlete = await QueryAsync(new GetAthleteProducts(athleteId, date));
            if (athlete == null)
            {
                return NotFound();
            }

            return Ok(athlete);
        }

        [HttpGet("{athleteId}/products/{productId}")]
        public async Task<IActionResult> GetProduct(int athleteId, int productId)
        {
            var athlete = await QueryAsync(new GetAthleteProduct(athleteId, productId));
            if (athlete == null)
            {
                return NotFound();
            }

            return Ok(athlete);
        }

        [HttpGet("{athleteId}/exercises")]
        public async Task<IActionResult> GetExercises(int athleteId, string dayName = "Monday")
        {
            var athlete = await QueryAsync(new GetAthleteExercises(athleteId, dayName));
            if (athlete is null)
            {
                return NotFound();
            }

            return Ok(athlete);
        }

        [HttpGet("{athleteId}/exercises/{exerciseId}")]
        public async Task<IActionResult> GetExercise(int athleteId, int exerciseId)
        {
            var athlete = await QueryAsync(new GetAthleteExercise(athleteId, exerciseId));
            if (athlete is null)
            {
                return NotFound();
            }

            return Ok(athlete);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateAthlete command)
        {
            await DispatchAsync(command);

            _logger.LogInfo($"Athlete with user id {command.UserId} added.");

            return CreatedAtAction(nameof(Get), new { athleteId = command.AthleteId }, command);
        
        }

        [HttpDelete("{athleteId}")]
        public async Task<IActionResult> Delete([FromBody]DeleteAthlete command)
        {
            await DispatchAsync(command);

            _logger.LogInfo($"Athlete with id {command.AthleteId} removed.");

            return NoContent();
        }
    }
}
