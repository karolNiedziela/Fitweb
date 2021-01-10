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
    public class UserProductServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IUserProductRepository> _userProductRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UserProductService _userProductService;

        public UserProductServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _userProductRepositoryMock = new Mock<IUserProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _userProductService =  new UserProductService(_userRepositoryMock.Object, _productRepositoryMock.Object, _userProductRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAsync_ShouldInvokeGetAsyncOnRepository()
        {
            await _userProductService.GetAsync(1);
            var role = new Role("User");

            var user = new User("user1", "user1@email.com", "secret", "salt", role);
            var product = new Product("product1", 100, 10, 10, 10);

            var userProduct = new AthleteProduct(user, product, 100);

            _userProductRepositoryMock.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync(userProduct);
            _userProductRepositoryMock.Verify(x => x.GetAsync(It.IsAny<int>()), Times.Once);
        }
        [Fact]
        public async Task DeleteAsync_ShouldInvokeDeleteAsyncOnRepository()
        {
            var role = new Role("User");

            var user = new User("user1", "user1@email.com", "secret", "salt", role);
            var product = new Product("product1", 100, 10, 10, 10);

            var userProduct = new AthleteProduct(user, product, 100);

            _userProductRepositoryMock.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync(userProduct);

            await _userProductService.DeleteAsync(1);
            _userProductRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<AthleteProduct>()), Times.Once);
        }

    }
}
