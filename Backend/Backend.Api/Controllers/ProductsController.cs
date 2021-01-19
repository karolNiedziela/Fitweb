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

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ApiControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILoggerManager _logger;

        public ProductsController(IDispatcher dispatcher, IProductService productService,
            ILoggerManager logger)
            : base(dispatcher)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet("{id:int}")]
        //GET : /api/products/id
        public async Task<IActionResult> Get(int id)
        {
            var product = await _dispatcher.QueryAsync(new GetProduct(id));
            if (product is null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        /*[HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            var product = await _productService.GetAsync(name);
            if (product is null)
            {
                return NotFound();
            }

            return Ok(product);
        }*/

        [HttpGet]
        //GET : /api/products
        public async Task<IActionResult> GetAll([FromQuery]GetProducts query)
        {
            var products = await _dispatcher.QueryAsync(query);

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

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery]SearchProducts query)
        {
            var results = await _dispatcher.QueryAsync(query);

            var metadata = new
            {
                results.TotalCount,
                results.PageSize,
                results.CurrentPage,
                results.TotalPages,
                results.HasNext,
                results.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(results);
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        //POST : /api/products
        public async Task<IActionResult> Post([FromBody]AddProduct command)
        {
            await DispatchAsync(command);

            _logger.LogInfo($"Product with name: {command.Name} added.");

            return CreatedAtAction(nameof(Get), new { id = command.Id }, command);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        //DELETE : /api/products/{id}
        public async Task<IActionResult> Delete([FromBody] DeleteProduct command)
        {
            await DispatchAsync(command);

            _logger.LogInfo($"Product with id: {command.ProductId} removed.");

            return NoContent();
        }

        //PUT: /api/products
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put([FromBody] UpdateProduct command)
        {
            await DispatchAsync(command);

            _logger.LogInfo($"Product with id: {command.ProductId} updated.");

            return CreatedAtAction(nameof(Get), new { id = command.ProductId }, command);
        }       
    }
}
