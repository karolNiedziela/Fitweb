using Backend.Infrastructure.EF;
using Backend.Infrastructure.Services.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public abstract class DataInitializer : IDataInitializer
    {
        protected readonly FitwebContext _context;
        protected readonly ILoggerManager _logger;

        protected DataInitializer(FitwebContext context, ILoggerManager logger)
        {
            _context = context;
            _logger = logger;
        }

        public abstract Task SeedAsync();
    }
}
