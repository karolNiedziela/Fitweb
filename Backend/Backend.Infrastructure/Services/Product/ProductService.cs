﻿using AutoMapper;
using Backend.Core.Entities;
using Backend.Core.Repositories;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<ProductDetailsDto> GetAsync(int id)
        {
            var product = await _productRepository.GetAsync(id);

            return _mapper.Map<Product, ProductDetailsDto>(product);
        }

        public async Task<ProductDetailsDto> GetAsync(string name)
        {
            var product = await _productRepository.GetAsync(name);

            return _mapper.Map<Product, ProductDetailsDto>(product);
        }

        public async Task<IEnumerable<ProductDetailsDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDetailsDto>>(products.OrderBy(p => p.Name));
        }

        public async Task<int> AddAsync(string name, double calories, double proteins,
            double carbohydrates, double fats, string categoryName)
        {
            var product = await _productRepository.GetAsync(name);
            if (product != null)
            {
                throw new ServiceException(Exceptions.ErrorCodes.ObjectAlreadyAdded,
                    $"Product with name: '{name}' already exists.");
            }

            product = new Product(name, calories, proteins, carbohydrates, fats, (GetCategory(categoryName)));

            await _productRepository.AddAsync(product);

            return product.Id;
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

        public async Task UpdateAsync(int id, string name, double calories, double proteins, 
            double carbohydrates, double fats, string categoryName)
        {
            var product = await _productRepository.GetAsync(id);
            if (product is null)
            {
                throw new ServiceException(Exceptions.ErrorCodes.ObjectNotFound,
                    $"Product with name: '{name}' was not found.");
            }

            product.SetName(name);
            product.SetCalories(calories);
            product.SetProteins(proteins);
            product.SetCarbohydrates(carbohydrates);
            product.SetFats(fats);
            product.CategoryOfProduct.Id = GetCategory(categoryName).Id;

            await _productRepository.UpdateAsync(product);
        }

        private static CategoryOfProduct GetCategory(string categoryOfProductName)
            => Enum.GetValues(typeof(CategoryOfProductId))
                            .Cast<CategoryOfProductId>()
                            .Select(cop => new CategoryOfProduct()
                            {
                                Id = (int)cop,
                                Name = cop
                            }).SingleOrDefault(cop => cop.Name.ToString() == categoryOfProductName);
    }
}