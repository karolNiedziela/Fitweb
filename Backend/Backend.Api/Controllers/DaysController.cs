﻿using Backend.Infrastructure.CommandQueryHandler.Commands;
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
    public class DaysController : Controller
    {
        private readonly FitwebContext _context;

        public DaysController(FitwebContext context) 
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var days = await _context.Days.ToListAsync();

            return Ok(days);
        }
    }
}
