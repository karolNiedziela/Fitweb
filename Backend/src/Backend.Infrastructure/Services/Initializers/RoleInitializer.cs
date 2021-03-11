using Backend.Core.Entities;
using Backend.Core.Enums;
using Backend.Infrastructure.EF;
using Backend.Infrastructure.Services.Logger;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public class RoleInitializer : DataInitializer
    {
        private readonly RoleManager<Role> _roleManager;

        public RoleInitializer(FitwebContext context, ILoggerManager logger,
            RoleManager<Role> roleManager) : base(context, logger)
        {
            _roleManager = roleManager;
        }

        public override async Task SeedAsync()
        {
            var roles = Enum.GetNames(typeof(RoleId)).ToList();

            foreach (string role in roles)
            {
                if(!await _context.Roles.AnyAsync(r => r.Name == role))
                {
                    await _roleManager.CreateAsync(new Role(role));
                }
            }

            _logger.LogInfo("Roles added by initializer.");
        }


    }
}
