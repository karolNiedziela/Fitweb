using AutoMapper;
using Backend.Core.Entities;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Helpers;
using Backend.Infrastructure.Mappers;
using Backend.Infrastructure.Repositories;
using Backend.Infrastructure.Services;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Fitweb.Tests.Unit.Services
{
    public class ProductServiceTests
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IProductService _sut;

        public ProductServiceTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _mapper = Substitute.For<IMapper>();
            _mapper = AutoMapperConfig.Initialize();
            _sut = new ProductService(_productRepository, _mapper);
        }

        [Fact]
        public async Task GetAsyncById_ShouldReturnProductDetailsDto()
        {
            // Arrange
            var product = new Product { Id = 1 };
            _productRepository.GetAsync(1).Returns(product);

            // Act
            var dto = await _sut.GetAsync(product.Id);

            // Assert
            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(product.Id);
            dto.ShouldBeOfType(typeof(ProductDetailsDto));
            await _productRepository.Received(1).GetAsync(product.Id);
        }

        [Fact]
        public async Task GetAsyncByName_ShouldReturnProductDetailsDto()
        {
            // Arrange
            var product = new Product { Name = "Banana" };
            _productRepository.GetAsync("Banana").Returns(product);

            // Act
            var dto = await _sut.GetAsync(product.Name);

            // Assert
            dto.ShouldNotBeNull();
            dto.Name.ShouldBe(product.Name);
            dto.ShouldBeOfType(typeof(ProductDetailsDto));
            await _productRepository.Received(1).GetAsync(product.Name);
        }

        [Fact]
        public async Task GetAllAsync_ShoudlReturnIEnumerableProductDetailsDto()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1 },
                new Product { Id = 1 }
            };

            var pageList = Substitute.For<PagedList<Product>>(products, 10, 10, 10);

            var paginationQuery = Substitute.For<PaginationQuery>();
            _productRepository.GetAllAsync(paginationQuery).Returns(pageList);

            // Act
            var dto = await _sut.GetAllAsync(paginationQuery);

            // Assert
            dto.ShouldNotBeNull();
            dto.ShouldBeOfType(typeof(PagedList<ProductDetailsDto>));
            dto.Count().ShouldBe(2);
        }

        [Fact]
        public async Task AddAsync_ShouldAddNewProduct()
        {
            // Arrange

            // Act
            await _sut.AddAsync("blabla", 100, 10, 10, 10, "Meat");

            // Assert
            await _productRepository.Received(1).AddAsync(Arg.Any<Product>());
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenProductDoesNotExist()
        {
            var exception = await Record.ExceptionAsync(() =>  _sut.DeleteAsync(1));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.ShouldBeOfType(typeof(ServiceException), "Product with id: 1 was not found.");
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenProductDoesNotExist()
        {
            var exception = await Record.ExceptionAsync(() => _sut.UpdateAsync(1, "blabla", 100, 10, 10, 10, "Meat"));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.ShouldBeOfType(typeof(ServiceException), "Product with id: 1 was not found.");
        }
    }
}
