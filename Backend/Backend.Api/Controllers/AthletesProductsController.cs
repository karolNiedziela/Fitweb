using Backend.Infrastructure.CommandHandler.Commands;
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
    [ApiController]
    [Route("athletes/products")]
    public class AthletesProductsController : ApiControllerBase
    {
        private readonly ILoggerManager _logger;

        public AthletesProductsController(ICommandDispatcher commandDispatcher, ILoggerManager logger)
            : base(commandDispatcher)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddAthleteProduct command)
        {
            await DispatchAsync(command);

            _logger.LogInfo($"Product with id: {command.ProductId} added to user with id: {command.UserId}.");

            return CreatedAtAction("GetProduct", "Athletes",
              new { userId = command.UserId, productId = command.ProductId }, command);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteAthleteProduct command)
        {
            await DispatchAsync(command);

            _logger.LogInfo($"Product with id: {command.ProductId} removed from user with id: {command.UserId}.");

            return NoContent();
        }
    }
}
