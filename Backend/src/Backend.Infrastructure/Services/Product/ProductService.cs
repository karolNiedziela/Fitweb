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

        public async Task<PagedList<ProductDetailsDto>> GetAllAsync(PaginationQuery paginationQuery)
        {
            var products =  await _productRepository.GetAllAsync(paginationQuery);

            var productsDTO = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDetailsDto>>(products).ToList();

            return new PagedList<ProductDetailsDto>(productsDTO, products.TotalCount, products.CurrentPage, products.PageSize);
        }

        public async Task<PagedList<ProductDetailsDto>> SearchAsync(PaginationQuery paginationQuery, string name, string category = null)
        {
            if (category is not null && CategoryOfProduct.GetCategory(category) is null)
            {
                throw new ServiceException(ErrorCodes.ObjectNotFound, $"{category} does not exist.");
            }

            var products = await _productRepository.SearchAsync(paginationQuery, name, category);

            var productsDto = _mapper.Map<IEnumerable<ProductDetailsDto>>(products).ToList();

            return new PagedList<ProductDetailsDto>(productsDto, products.TotalCount, products.CurrentPage, products.PageSize);
        }

        public async Task<int> AddAsync(string name, double calories, double proteins,
            double carbohydrates, double fats, string categoryName)
        {
            var product = await _productRepository.GetAsync(name);
            if (product is not null)
            {
                throw new ServiceException(Exceptions.ErrorCodes.ObjectAlreadyAdded,
                    $"Product with name: '{name}' already exists.");
            }

            product = new Product(name, calories, proteins, carbohydrates, fats, CategoryOfProduct.GetCategory(categoryName));

            await _productRepository.AddAsync(product);

            return product.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _productRepository.GetAsync(id);
            if (product is null)
            {
                throw new ServiceException(ErrorCodes.ObjectNotFound,
                    $"Product with id: '{id}' was not found.");
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

            if (await _productRepository.AnyAsync(p => p.Name == name))
            {
                if (product.Name != name)
                {
                    throw new ServiceException(ErrorCodes.ObjectAlreadyAdded, $"Product with '{name}' already exists.");
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
