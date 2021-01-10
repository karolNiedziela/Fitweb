using Backend.Core.Entities;
using Backend.Infrastructure.CommandHandler.Commands;
using Backend.Infrastructure.EF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Infrastructure.Extensions;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesOfProductController : Controller
    {
        private readonly FitwebContext _context;

        public CategoriesOfProductController(FitwebContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _context.CategoriesOfProduct.ToListAsync();

            return Ok(categories);
        }


    }
}
