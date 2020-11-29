using AutoMapper;
using Backend.Core.Domain;
using Backend.Core.Repositories;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
        }

        public async Task AddAsync(string name, double calories, double proteins,
            double carbohydrates, double fats)
        {
            var product = await _productRepository.GetAsync(name);
            if (product != null)
            {
                throw new ServiceException(Exceptions.ErrorCodes.ObjectAlreadyAdded,
                    $"Product with name: '{name}' already exists.");
            }

            product = new Product(name, calories, proteins, carbohydrates, fats);
            await _productRepository.AddAsync(product);
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _productRepository.GetAsync(id);
            if (product == null)
            {
                throw new ServiceException(Exceptions.ErrorCodes.ObjectNotFound,
                    $"Product with id: '{id}' was not found.");
            }

            await _productRepository.DeleteAsync(product);
        }

        public async Task DeleteAsync(string name)
        {
            var product = await _productRepository.GetAsync(name);
            if (product == null)
            {
                throw new ServiceException(Exceptions.ErrorCodes.ObjectNotFound,
                    $"Product with name: '{name}' was not found.");
            }

            await _productRepository.DeleteAsync(product);
        }

        public async Task UpdateAsync(string name, double calories, double proteins, double carbohydrates, double fats)
        {
            var product = await _productRepository.GetAsync(name);
            if (product == null)
            {
                throw new ServiceException(Exceptions.ErrorCodes.ObjectNotFound,
                    $"Product with name: '{name}' was not found.");
            }

            product.SetName(name);
            product.SetCalories(calories);
            product.SetProteins(proteins);
            product.SetCarbohydrates(carbohydrates);
            product.SetFats(fats);

            await _productRepository.UpdateAsync(product);
        }

        
    }
}
