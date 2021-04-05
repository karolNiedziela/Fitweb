using AutoMapper;
using Backend.Core.Entities;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Exceptions;
using Backend.Core.Helpers;
using Backend.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Infrastructure.Extensions;
using Backend.Core.Exceptions;

namespace Backend.Infrastructure.Services
{
    public class ProductService : IProductService
    {      
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<ProductDto> GetAsync(int id)
        {
            var product = await _productRepository.GetAsync(id);

            return _mapper.Map<Product, ProductDto>(product);
        }

        public async Task<ProductDto> GetAsync(string name)
        {
            var product = await _productRepository.GetAsync(name);

            return _mapper.Map<Product, ProductDto>(product);
        }

        public async Task<PageResultDto<ProductDto>> GetAllAsync(string name, string category, 
            PaginationQuery paginationQuery)
        {
            var products = await _productRepository.GetAllAsync(name, category, paginationQuery);

            return _mapper.Map<PagedList<Product>, PageResultDto<ProductDto>>(products);
        }

        public async Task<int> AddAsync(string name, double calories, double proteins,
            double carbohydrates, double fats, string categoryName)
        {
            var product = await _productRepository.CheckIfExistsAsync(name);

            product = new Product(name, calories, proteins, carbohydrates, fats, CategoryOfProduct.GetCategory(categoryName));

            await _productRepository.AddAsync(product);

            return product.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _productRepository.GetOrFailAsync(id);

            await _productRepository.DeleteAsync(product);
        }

        public async Task UpdateAsync(int id, string name, double calories, double proteins, 
            double carbohydrates, double fats, string categoryName)
        {
            var product = await _productRepository.GetOrFailAsync(id);

            // If product has the same name, ensure that name will be searching for product with this name 
            if (await _productRepository.AnyAsync(p => p.Name == name))
            {
                if (product.Name != name)
                {
                    throw new NameInUseException(nameof(Product), name);
                }
            }

            product.SetName(name);
            product.SetCalories(calories);
            product.SetProteins(proteins);
            product.SetCarbohydrates(carbohydrates);
            product.SetFats(fats);
            product.CategoryOfProduct.Id = CategoryOfProduct.GetCategory(categoryName).Id;

            await _productRepository.UpdateAsync(product);
        }
    }
}
