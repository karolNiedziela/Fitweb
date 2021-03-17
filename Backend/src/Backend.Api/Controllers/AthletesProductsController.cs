using Backend.Infrastructure.CommandQueryHandler;
using Backend.Infrastructure.CommandQueryHandler.Commands;
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
    [ApiController]
    [Route("athlete/products")]
    [Authorize]
    public class AthletesProductsController : ApiControllerBase
    {
        private readonly ILoggerManager _logger;

        public AthletesProductsController(IDispatcher dispatcher, ILoggerManager logger)
            : base(dispatcher)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AthleteDto>> GetAll(DateTime? date = null)
        {
            var athlete = await QueryAsync(new GetAthleteProducts(UserId, date));
            if (athlete == null)
            {
                return NotFound();
            }

            return Ok(athlete);
        }

        [HttpGet("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AthleteDto>> Get(int productId)
        {
            var athlete = await QueryAsync(new GetAthleteProduct(UserId, productId));
            if (athlete == null)
            {
                return NotFound();
            }

            return Ok(athlete);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody]AddAthleteProduct command)
        {
            await DispatchAsync(command);

            _logger.LogInfo($"Product with id: {command.ProductId} added to athlete with user id: {command.UserId}.");

            return CreatedAtAction(nameof(Get),
              new { productId = command.ProductId }, command);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromBody] DeleteAthleteProduct command)
        {
            await DispatchAsync(command);

            _logger.LogInfo($"Product with id: {command.ProductId} removed from athlete with user id: {command.UserId}.");

            return NoContent();
        }
    }
}
