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
using Backend.Tests.Unit.Fixtures;

namespace Backend.Tests.Unit.Services
{
    public class ProductServiceTests : IClassFixture<FitwebFixture>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IProductService _sut;
        FitwebFixture _fixture;

        public ProductServiceTests(FitwebFixture fixture)
        {
            _fixture = fixture;
            _productRepository = Substitute.For<IProductRepository>();
            _mapper = Substitute.For<IMapper>();
            _mapper = AutoMapperConfig.Initialize();
            _sut = new ProductService(_productRepository, _mapper);
        }

        [Fact]
        public async Task GetAsyncById_ShouldReturnProductDetailsDto()
        {
            // Arrange
            var product = _fixture.FitwebContext.Products.SingleOrDefault(p => p.Id == 1);
            _productRepository.GetAsync(product.Id).Returns(product);

            // Act
            var dto = await _sut.GetAsync(product.Id);

            // Assert
            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(product.Id);
            dto.Name.ShouldBe(product.Name);
            dto.ShouldBeOfType(typeof(ProductDetailsDto));
            await _productRepository.Received(1).GetAsync(product.Id);
        }

        [Fact]
        public async Task GetAsyncByName_ShouldReturnProductDetailsDto()
        {
            // Arrange
            var product = _fixture.FitwebContext.Products.SingleOrDefault(p => p.Name == "product1");
            _productRepository.GetAsync(product.Name).Returns(product);

            // Act
            var dto = await _sut.GetAsync(product.Name);

            // Assert
            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(product.Id);
            dto.Name.ShouldBe(product.Name);
            dto.ShouldBeOfType(typeof(ProductDetailsDto));
            await _productRepository.Received(1).GetAsync(product.Name);
        }

        [Fact]
        public async Task GetAllAsync_ShoudlReturnPagedListOfProductDetailsDto()
        {
            // Arrange
            var products = _fixture.FitwebContext.Products.ToList();
            var paginationQuery = Substitute.For<PaginationQuery>();
            var pageList = Substitute.For<PagedList<Product>>(products, 10, 10, 10);
            _productRepository.GetAllAsync(paginationQuery).Returns(pageList);


            // Act
            var dto = await _sut.GetAllAsync(paginationQuery);

            // Assert
            dto.ShouldNotBeNull();
            dto.ShouldBeOfType(typeof(PagedList<ProductDetailsDto>));
            dto.Count().ShouldBe(products.Count);
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
            var product = _fixture.FitwebContext.Products.SingleOrDefault(p => p.Id == 1);
            _productRepository.GetAsync(product.Name).Returns(product);

            var exception = await Record.ExceptionAsync(() => 
                _sut.AddAsync(product.Name, 500, 25, 70, 5, "Meat"));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.ShouldBeOfType(typeof(ServiceException), $"Product with name: 'product2' already exists.");
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteProduct_IfProductExists()
        {
            var product = _fixture.FitwebContext.Products.SingleOrDefault(p => p.Id == 1);
            _productRepository.GetAsync(product.Id).Returns(product);

            await _sut.DeleteAsync(1);

            await _productRepository.Received(1).DeleteAsync(Arg.Is(product));
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
        public async Task UpdateAsync_ShouldUpdateProduct_WhenProductExistsAndDataIsValid()
        {
            var product = _fixture.FitwebContext.Products.SingleOrDefault(p => p.Id == 1);
            _productRepository.GetAsync(product.Id).Returns(product);

            await _sut.UpdateAsync(1, "blabla", 100, 10, 10, 10, "Meat");

            await _productRepository.Received(1).UpdateAsync(Arg.Any<Product>());
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenProductDoesNotExist()
        {
            var exception = await Record.ExceptionAsync(() => _sut.UpdateAsync(1, "blabla", 100, 10, 10, 10, "Meat"));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.ShouldBeOfType(typeof(ServiceException), "Product with id: 1 was not found.");
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenProductExistsButNameIsNotUnique()
        {
            var product = _fixture.FitwebContext.Products.SingleOrDefault(p => p.Id == 1);
            _productRepository.GetAsync(product.Id).Returns(product);

            _productRepository.AnyAsync(p => p.Name == "someName").ReturnsForAnyArgs(true);

            var exception = await Record.ExceptionAsync(() => _sut.UpdateAsync(1, "someName", 100, 10, 10, 10, "Meat"));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.ShouldBeOfType(typeof(ServiceException), "Product with '{someName}' already exists.");
        }
    }
}
