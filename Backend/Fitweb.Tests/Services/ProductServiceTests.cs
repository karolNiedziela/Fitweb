using AutoMapper;
using Backend.Core.Domain;
using Backend.Core.Repositories;
using Backend.Infrastructure.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Fitweb.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _productService = new ProductService(_productRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task when_calling_get_async_and_product_exists_it_should_invoke_product_repository_get_async()
        {
            await _productService.GetAsync("product1");

            var product = new Product("product1", 100, 10, 10, 10);

            _productRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(product);

            _productRepositoryMock.Verify(x => x.GetAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task add_async_should_invoke_add_async_on_repository()
        {

            await _productService.AddAsync("product1", 100, 10, 10, 10);

            _productRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task when_calling_get_async_and_product_does_not_exist_it_should_invoke_product_repository_get_async()
        {
            await _productService.GetAsync("product1");

            _productRepositoryMock.Setup(x => x.GetAsync("product1")).ReturnsAsync(() => null);

            _productRepositoryMock.Verify(x => x.GetAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task when_calling_update_async_and_product_exist_it_should_invoke_product_repository_update_async()
        {
            await _productService.GetAsync("product1");

            var product = new Product("product1", 100, 10, 10, 10);

            _productRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(product);

            await _productService.UpdateAsync(product.Name, product.Calories, product.Proteins, product.Carbohydrates, product.Fats);
            _productRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task when_calling_delete_async_and_product_exist_it_should_invoke_product_repository_delete_async()
        {
            await _productService.GetAsync("product1");

            var product = new Product("product1", 100, 10, 10, 10);

            _productRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(product);

            await _productService.DeleteAsync(product.Name);
            _productRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Product>()), Times.Once);
        }

    }
}
