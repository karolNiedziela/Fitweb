using Backend.Core.Entities;
using Backend.Core.Repositories;
using Backend.Infrastructure.EF;
using Backend.Infrastructure.Services.Logger;
using Backend.Infrastructure.Utilities.Csv;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public class ProductInitializer : DataInitializer
    {
        private readonly ICsvLoader<Product, ProductMap> _loader;
        private readonly IProductRepository _productRepository;

        public ProductInitializer(FitwebContext context, ILoggerManager logger,
            ICsvLoader<Product, ProductMap> loader, IProductRepository productRepository) : base(context, logger)
        {
            _loader = loader;
            _productRepository = productRepository;
        }


        public override async Task SeedAsync()
        {
            if (await _context.Products.AnyAsync())
            {
                return;
            }

            var stringPath = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
            stringPath += @"/Files/products.csv";

            var products = _loader.LoadCsvAsync(stringPath);
            
            await _productRepository.AddRangeAsync(products);

            _logger.LogInfo("Products added from products.csv");
        }

    }
}
