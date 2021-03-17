using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.CommandQueryHandler;
using Backend.Infrastructure.Services;
using Backend.Infrastructure.Services.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Backend.Infrastructure.CommandQueryHandler.Queries;
using Backend.Infrastructure.CommandQueryHandler.Queries.Products;
using Microsoft.AspNetCore.Http;
using Backend.Infrastructure.DTO;
using System.Collections.Generic;
using Backend.Infrastructure.Auth;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ApiControllerBase
    {
        private readonly ILoggerManager _logger;

        public ProductsController(IDispatcher dispatcher, ILoggerManager logger)
            : base(dispatcher)
        {
            _logger = logger;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //GET : /api/products/id
        public async Task<ActionResult<ProductDto>> Get(int id)
        {
            var product = await QueryAsync(new GetProduct(id));
            if (product is null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet("{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> Get(string name)
        {
            var product = await QueryAsync(new GetProductByName(name));
            if (product is null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet]
        //GET : /api/products
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll([FromQuery]GetProducts query)
        {
            var products = await QueryAsync(query);

            var metadata = new
            {
                products.TotalCount,
                products.PageSize,
                products.CurrentPage,
                products.TotalPages,
                products.HasNext,
                products.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(products);
        }

        [HttpPost]
        [Authorize(Policy = PolicyNames.AdminOnly)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        //POST : /api/products
        public async Task<ActionResult> Post([FromBody]AddProduct command)
        {
            await DispatchAsync(command);

            _logger.LogInfo($"Product with name: {command.Name} added.");

            return CreatedAtAction(nameof(Get), new { id = command.Id }, command);
        }

        [HttpDelete]
        [Authorize(Policy = PolicyNames.AdminOnly)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        //DELETE : /api/products/{id}
        public async Task<ActionResult> Delete([FromBody] DeleteProduct command)
        {
            await DispatchAsync(command);

            _logger.LogInfo($"Product with id: {command.ProductId} removed.");

            return NoContent();
        }

        //PUT: /api/products
        [HttpPut]
        [Authorize(Policy = PolicyNames.AdminOnly)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Put([FromBody] UpdateProduct command)
        {
            await DispatchAsync(command);

            _logger.LogInfo($"Product with id: {command.ProductId} updated.");

            return Ok();
        }       
    }
}
