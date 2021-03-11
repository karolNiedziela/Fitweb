using Backend.Infrastructure.Auth;
using Backend.Infrastructure.CommandQueryHandler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ApiControllerBase
    {
        public TestsController(IDispatcher dispatcher) : base(dispatcher)
        {
        }

        [Authorize(Policy = PolicyNames.AdminOnly)]
        [HttpGet]
        public IActionResult Get()
        {

            var claims = User.Claims.ToList();

            return Ok(new { Information = "information" });
        }
    }
}
