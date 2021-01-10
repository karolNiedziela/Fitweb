using Backend.Core.Entities;
using Backend.Core.Repositories;
using Backend.Infrastructure.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public class DataInitializer : IDataInitializer
    {
        private readonly IProductInitializer _productInitializer;
        private readonly FitwebContext _context;

        public DataInitializer(IProductInitializer productInitializer, FitwebContext context)
        {
            _productInitializer = productInitializer;
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (_context.Products.Any())
            {
                return;
            }
            await _productInitializer.LoadFromCsv();
        }
    }
}
