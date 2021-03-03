using Backend.Infrastructure.CommandQueryHandler;
using Backend.Infrastructure.CommandQueryHandler.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfirmEmailController : ApiControllerBase
    {
        public ConfirmEmailController(IDispatcher dispatcher) : base(dispatcher)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromQuery]ConfirmEmail query)
        {
            if (query.UId == null || query.Code == null)
            {
                return NotFound();
            }

            var result = await QueryAsync(query);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
