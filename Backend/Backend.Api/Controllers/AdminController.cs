using Backend.Infrastructure.CommandHandler.Commands;
using Backend.Infrastructure.Services;
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
    public class AdminController : ApiControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(ICommandDispatcher commandDispatcher, IAdminService adminService) : base(commandDispatcher)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _adminService.CheckIfAdminExists();

            return Ok(result);
        }
    }
}
