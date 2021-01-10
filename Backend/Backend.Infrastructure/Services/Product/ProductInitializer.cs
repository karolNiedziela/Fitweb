using Backend.Core.Entities;
using Backend.Core.Repositories;
using Backend.Infrastructure.Services.File;
using Backend.Infrastructure.Utilities.Csv;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public class ProductInitializer : IProductInitializer
    {
        private readonly IProductService _productService;
        private readonly ICsvLoader<Product, ProductMap> _loader;
        private readonly IProductRepository _productRepository;

        public ProductInitializer(IProductService productService, ICsvLoader<Product, ProductMap> loader,
            IProductRepository productRepository)
        {
            _productService = productService;
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
