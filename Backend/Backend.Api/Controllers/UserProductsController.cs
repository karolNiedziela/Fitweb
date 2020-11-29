using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Backend.Infrastructure.Commands;
using Backend.Infrastructure.Commands.UserProducts;
using Backend.Infrastructure.EF;
using Backend.Infrastructure.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProductsController : ApiControllerBase
    {
        private readonly IUserProductService _userProductService;

        public UserProductsController(ICommandDispatcher commandDispatcher, IUserProductService userProductService) 
            : base(commandDispatcher)
        {
            _userProductService = userProductService;
        }

        //GET: /api/userproducts
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userProducts = await _userProductService.GetAllAsync();

            return Json(userProducts);
        }

        //GET: /api/userproducts/{userId}
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserProducts(int userId)
        {
            DateTime dateTime = DateTime.Today;
            DateTime dateOnly = dateTime.Date;
            var userProducts = await _userProductService.GetAllUserProductsFromDay(userId, dateOnly.ToString());

            return Json(userProducts);
        }

        //GET: /api/userproducts/{userId}/{date}
        [HttpGet("{userId}/{date}")]
        public async Task<IActionResult> GetUserProducts([FromQuery]int userId, [FromQuery]string date)
        { 
            var userProducts = await _userProductService.GetAllUserProductsFromDay(userId, date);

            return Json(userProducts);
        }



        //POST: /api/userproducts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddUserProduct command)
        {

            await DispatchAsync(command);

            return Ok();
        }

        //PUT: /api/userproducts/
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]UpdateUserProduct command)
        {
            await DispatchAsync(command);

            return Ok();
        }

        //DELETE: /api/userproducts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userProductService.DeleteAsync(id);

             return NoContent();
        }
    }
}
