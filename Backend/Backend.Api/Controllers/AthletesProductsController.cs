using Backend.Infrastructure.CommandHandler.Commands;
using Backend.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
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

        public AthletesProductsController(ICommandDispatcher commandDispatcher)
            : base(commandDispatcher)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddAthleteProduct command)
        {
            await DispatchAsync(command);

            return CreatedAtAction("GetProduct", "Athletes",
              new { userId = command.UserId, productId = command.ProductId }, command);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteAthleteProduct command)
        {
            await DispatchAsync(command);

            return NoContent();
        }
    }
}
