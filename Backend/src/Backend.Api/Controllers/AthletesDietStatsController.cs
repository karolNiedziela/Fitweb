using Backend.Infrastructure.CommandQueryHandler;
using Backend.Infrastructure.CommandQueryHandler.Queries.Athletes;
using Backend.Infrastructure.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Api.Controllers
{
    [ApiController]
    [Route("api/athletes/dietstats")]
    [Authorize]
    public class AthletesDietStatsController : ApiControllerBase
    {
        public AthletesDietStatsController(IDispatcher dispatcher) : base(dispatcher)
        {
        }

        [HttpGet]
        public async Task<ActionResult<AthleteDto>> GetAsync(DateTime? date)
        {
            var athlete = await QueryAsync(new GetDietStats(UserId, date));


            return Ok(athlete);
        }
    }
}
