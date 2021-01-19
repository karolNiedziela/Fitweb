using Backend.Infrastructure.CommandQueryHandler;
using Backend.Infrastructure.CommandQueryHandler.Commands;
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
        private readonly IAthleteService _athleteService;
        private readonly ILoggerManager _logger;

        public AthletesController(IDispatcher dispatcher, IAthleteService athleteService,
            ILoggerManager logger) : base(dispatcher)
        {
            _athleteService = athleteService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var athletes = await _athleteService.GetAllAsync();

            return Ok(athletes);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(int userId)
        {
            var athlete = await _athleteService.GetAsync(userId);
            if (athlete == null)
            {
                return NotFound();
            }

            return Ok(athlete);
        }

        [HttpGet("{userId}/products")]
        public async Task<IActionResult> GetProducts(int userId)
        {
            var athlete = await _athleteService.GetProductsAsync(userId);
            if (athlete == null)
            {
                return NotFound();
            }

            return Ok(athlete);
        }

        [HttpGet("{userId}/products/{productId}")]
        public async Task<IActionResult> GetProduct(int userId, int productId)
        {
            var athlete = await _athleteService.GetProductAsync(userId, productId);
            if (athlete == null)
            {
                return NotFound();
            }

            return Ok(athlete);
        }

        [HttpGet("{userId}/exercises")]
        public async Task<IActionResult> GetExercises(int userId)
        {
            var athlete = await _athleteService.GetExercisesAsync(userId);
            if (athlete is null)
            {
                return NotFound();
            }

            return Ok(athlete);
        }

        [HttpGet("{userId}/exercises/{exerciseId}")]
        public async Task<IActionResult> GetExercise(int userId, int exerciseId)
        {
            var athlete = await _athleteService.GetExerciseAsync(userId, exerciseId);
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

            return CreatedAtAction(nameof(Get), new { userId = command.UserId }, command);
        
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(int userId)
        {
            await _athleteService.DeleteAsync(userId);

            _logger.LogInfo($"Athlete with user id {userId} removed.");

            return NoContent();
        }
    }
}
