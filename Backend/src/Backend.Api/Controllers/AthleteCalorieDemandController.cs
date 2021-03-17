using Backend.Infrastructure.CommandQueryHandler;
using Backend.Infrastructure.CommandQueryHandler.Commands.Athletes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Api.Controllers
{
    [ApiController]
    [Route("athlete/caloricdemand")]
    [Authorize]
    public class AthleteCalorieDemandController : ApiControllerBase
    {
        public AthleteCalorieDemandController(IDispatcher dispatcher) : base(dispatcher)
        {
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]SetCaloricDemand command)
        {
            await DispatchAsync(command);

            return CreatedAtAction("Get", "Athletes", new { userId = command.UserId }, command);
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody]UpdateCaloricDemand command)
        {
            await DispatchAsync(command);

            return NoContent();
        }
    }
}
