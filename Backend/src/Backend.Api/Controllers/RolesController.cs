using Backend.Infrastructure.EF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : Controller
    {
        private readonly FitwebContext _context;

        public RolesController(FitwebContext context)
        {
            _context = context;
        }

        //GET : /api/days
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAll()
        {
            var roles = await _context.Roles.ToListAsync();

            return Ok(roles);
        }
    }
}
