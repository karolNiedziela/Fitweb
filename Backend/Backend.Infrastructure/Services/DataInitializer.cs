using Backend.Core.Entities;
using Backend.Infrastructure.EF;
using Backend.Infrastructure.Services.Account;
using Backend.Infrastructure.Services.Logger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public class DataInitializer : IDataInitializer
    {
        private readonly IProductInitializer _productInitializer;
        private readonly FitwebContext _context;
        private readonly IExerciseInitializer _exerciseInitializer;
        private readonly ILoggerManager _logger;
        private readonly IAccountService _accountService;

        public DataInitializer(IProductInitializer productInitializer, IExerciseInitializer exerciseInitializer, 
            FitwebContext context, ILoggerManager logger, IAccountService accountService)
        {
            _context = context;
            _productInitializer = productInitializer;
            _exerciseInitializer = exerciseInitializer;
            _accountService = accountService;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            if (!_context.Products.Any())
            {
                await _productInitializer.LoadFromCsv();
                _logger.LogInfo("Products added from products.csv");
            }

            if (!_context.Exercises.Any())
            {
                await _exerciseInitializer.LoadFromCsv();
                _logger.LogInfo("Exercises added from exercises.csv");
            }

            if (!await _context.Users.AnyAsync(u => u.UserRoles.Any(ur => ur.Role.Name == Role.GetRole("Admin").Name)))
            {
                await _accountService.SignUpAsync("admin123", "admin123@email.com", "admin123", "Admin");
                _logger.LogInfo($"User with username admin123 and role Admin added");
            }
        }
    }
}
