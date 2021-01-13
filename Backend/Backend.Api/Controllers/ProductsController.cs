using Backend.Infrastructure.CommandHandler.Commands;
using Backend.Infrastructure.Helpers;
using Backend.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ApiControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(ICommandDispatcher commandDispatcher, IProductService productService)
            : base(commandDispatcher)
        {
            _productService = productService;
        }

        [HttpGet("{id}")]
        //GET : /api/products/id
        public async Task<IActionResult> Get(int id)
        {
            var product = await _productService.GetAsync(id);
            if (product is null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            var product = await _productService.GetAsync(name);
            if (product is null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet]
        //GET : /api/products
        public async Task<IActionResult> GetAll([FromQuery]PaginationQuery paginationQuery)
        {
            var products = await _productService.GetAllAsync(paginationQuery);

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
        public async Task<IActionResult> Search([FromQuery]PaginationQuery paginationQuery, string name, string category = null)
        {
            var results = await _productService.SearchAsync(paginationQuery, name, category);

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
      //  [Authorize(Roles ="Admin")]
        //POST : /api/products
        public async Task<IActionResult> Post([FromBody] AddProduct command)
        {
            await DispatchAsync(command);

            return CreatedAtAction(nameof(Get), new { id = command.Id }, command);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        //DELETE : /api/products/{id}
        public async Task<IActionResult> Delete([FromBody] DeleteProduct command)
        {
            await DispatchAsync(command);

            return NoContent();
        }

        //PUT: /api/products
        [HttpPut]
    //    [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put([FromBody] UpdateProduct command)
        {
            await DispatchAsync(command);

            return CreatedAtAction(nameof(Get), new { id = command.Id }, command);
        }       
    }
}
