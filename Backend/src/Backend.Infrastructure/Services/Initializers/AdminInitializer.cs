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

namespace Backend.Infrastructure.Services.Initializers
{
    public class AdminInitializer : DataInitializer
    {
        private readonly UserManager<User> _userManager;

        public AdminInitializer(FitwebContext context, ILoggerManager logger,
            UserManager<User> userManager) : base(context, logger)
        {
            _userManager = userManager;
        }

        public override async Task SeedAsync()
        {
            if (await _context.Users.AnyAsync(u => u.UserName == "admin"))
            {
                return;
            }

            var admin = new User("admin", "admin@email.com");

            await _userManager.CreateAsync(admin, "adminSecret");
            await _userManager.AddToRoleAsync(admin, RoleId.Admin.ToString());

            _logger.LogInfo($"Admin with username {admin.UserName} added.");

        }
    }
}
