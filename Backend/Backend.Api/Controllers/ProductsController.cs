using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Infrastructure.Commands;
using Backend.Infrastructure.Commands.Products;
using Backend.Infrastructure.Services;
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

        [HttpGet("{name}")]
        //GET : /api/products/name
        public async Task<IActionResult> Get(string name)
        {
            var product = await _productService.GetAsync(name);
            if (product == null)
            {
                return NotFound();
            }

            return Json(product);
        }

        [HttpGet]
        //GET : /api/products
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            if (products == null)
            {
                return NotFound();
            }

            return Json(products);
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        //POST : /api/products
        public async Task<IActionResult> Post(AddProduct command)
        {
            await DispatchAsync(command);

            return Created($"api/products/{command.Name}", null);
        }

        //PUT: /api/products
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(UpdateProduct command)
        {
            await DispatchAsync(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        //DELETE : /api/products/{id}
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);

            return NoContent();
        }
    }
}
