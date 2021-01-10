using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Infrastructure.CommandHandler.Commands;
using Backend.Infrastructure.Services;
using Backend.Infrastructure.Services.File;
using Backend.Infrastructure.Utilities.Csv;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        //GET : /api/products/name
        public async Task<IActionResult> Get(int id)
        {
            var product = await _productService.GetAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet]
        //GET : /api/products
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();

            return Ok(products);
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
