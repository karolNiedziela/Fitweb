using Backend.Infrastructure.Auth;
using Backend.Infrastructure.CommandQueryHandler;
using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.CommandQueryHandler.Commands.Athletes;
using Backend.Infrastructure.CommandQueryHandler.Queries.Athletes;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Services;
using Backend.Infrastructure.Services.Logger;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AthleteDto>> Get(int userId)
        {
            var athlete = await QueryAsync(new GetAthlete(userId));
            if (athlete == null)
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

            return CreatedAtAction(nameof(Get), new { userId = command.UserId }, command);
        
        }

        [HttpDelete]
        [Authorize(Policy = PolicyNames.AdminOnly)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Delete([FromBody]DeleteAthlete command)
        {
            await DispatchAsync(command);

            _logger.LogInfo($"Athlete with user id {command.UserId} removed.");

            return NoContent();
        }
    }
}
