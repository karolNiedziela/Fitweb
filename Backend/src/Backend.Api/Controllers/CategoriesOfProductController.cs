using Backend.Core.Entities;
using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.EF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Infrastructure.Extensions;
using Backend.Infrastructure.CommandQueryHandler.Queries;
using Backend.Infrastructure.CommandQueryHandler;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesOfProductController : ApiControllerBase
    {
        public CategoriesOfProductController(IDispatcher dispatcher) : base(dispatcher)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAll()
        {
            var categories = await QueryAsync(new GetCategories());

            return Ok(categories);
        }


    }
}
