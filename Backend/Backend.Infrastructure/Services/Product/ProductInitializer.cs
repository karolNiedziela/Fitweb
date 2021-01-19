using Backend.Core.Entities;
using Backend.Core.Repositories;
using Backend.Infrastructure.Utilities.Csv;
using System.IO;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public class ProductInitializer : IProductInitializer
    {
        private readonly ICsvLoader<Product, ProductMap> _loader;
        private readonly IProductRepository _productRepository;

        public ProductInitializer(ICsvLoader<Product, ProductMap> loader, IProductRepository productRepository)
        {
            _loader = loader;
            _productRepository = productRepository;
        }

        public async Task LoadFromCsv()
        {
            var stringPath = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
            stringPath += @"/Files/products.csv";

            var products = _loader.LoadCsvAsync(stringPath);

            await _productRepository.AddRangeAsync(products);
        }
    }
}
