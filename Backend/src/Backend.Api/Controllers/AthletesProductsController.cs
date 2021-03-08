using Backend.Infrastructure.CommandQueryHandler;
using Backend.Infrastructure.CommandQueryHandler.Commands;
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
    [ApiController]
    [Route("athletes/products")]
    public class AthletesProductsController : ApiControllerBase
    {
        private readonly ILoggerManager _logger;

        public AthletesProductsController(IDispatcher dispatcher, ILoggerManager logger)
            : base(dispatcher)
        {
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody]AddAthleteProduct command)
        {
            await DispatchAsync(command);

            _logger.LogInfo($"Product with id: {command.ProductId} added to athlete with id: {command.AthleteId}.");

            return CreatedAtAction("GetProduct", "Athletes",
              new { athleteId = command.AthleteId, productId = command.ProductId }, command);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromBody] DeleteAthleteProduct command)
        {
            await DispatchAsync(command);

            _logger.LogInfo($"Product with id: {command.ProductId} removed from athlete with id: {command.AthleteId}.");

            return NoContent();
        }
    }
}
