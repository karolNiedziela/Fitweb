using AutoMapper;
using Backend.Core.Entities;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.EF;
using Backend.Infrastructure.Exceptions;
using Backend.Core.Helpers;
using Backend.Infrastructure.Mappers;
using Backend.Core.Repositories;
using Backend.Infrastructure.Services;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Backend.Infrastructure.Repositories;

namespace Fitweb.Tests.Unit.Services
{
    public class ProductServiceTests : IClassFixture<FitwebSeedDataFixture>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IProductService _sut;
        FitwebSeedDataFixture _fixture;

        public ProductServiceTests(FitwebSeedDataFixture fixture)
        {
            _fixture = fixture;
            _productRepository = Substitute.For<IProductRepository>();
            _mapper = Substitute.For<IMapper>();
            _mapper = AutoMapperConfig.Initialize();
            _sut = Substitute.For<ProductService>(_productRepository, _mapper);
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
                new Product { Id = 2 }
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
        public async Task AddAsync_ShouldThrowException_WhenProductNameIsNotUnique()
        {
            var productRepository = Substitute.For<ProductRepository>(_fixture.FitwebContext);
            var productService = Substitute.For<ProductService>(productRepository, _mapper);

            var exception = await Record.ExceptionAsync(() => productService.AddAsync("product2", 500, 25, 70, 5, "Meat"));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.ShouldBeOfType(typeof(ServiceException), $"Product with name: 'product2' already exists.");
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteProduct_IfProductExists()
        {
            var productRepository = Substitute.For<ProductRepository>(_fixture.FitwebContext);
            var productService = Substitute.For<ProductService>(productRepository, _mapper);

            await productService.DeleteAsync(1);

            var product = await productRepository.GetAsync(1);
            product.ShouldBeNull();
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenProductDoesNotExist()
        {
            var exception = await Record.ExceptionAsync(() => _sut.DeleteAsync(1));

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
