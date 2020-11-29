using Backend.Infrastructure.Commands;
using Backend.Infrastructure.EF;
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
    public class DaysController : ApiControllerBase
    {
        private readonly FitwebContext _context;

        public DaysController(ICommandDispatcher commandDispatcher, FitwebContext context) 
            : base(commandDispatcher)
        {
            _context = context;
        }

        //GET : /api/days
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var days = await _context.Days.ToListAsync();

            return Json(days);
        }
    }
}
