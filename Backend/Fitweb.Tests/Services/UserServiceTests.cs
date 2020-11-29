using AutoMapper;
using Backend.Core.Domain;
using Backend.Core.Repositories;
using Backend.Infrastructure.Repositories;
using Backend.Infrastructure.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Fitweb.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> UserRepositoryMock;
        private readonly Mock<IEncrypter> EncrypterMock;
        private readonly Mock<IMapper> MapperMock;
        private readonly Mock<IRoleRepository> RoleRepositoryMock;
        private readonly UserService UserService;

        public UserServiceTests()
        {
            UserRepositoryMock = new Mock<IUserRepository>();
            EncrypterMock = new Mock<IEncrypter>();
            EncrypterMock.Setup(x => x.GetSalt(It.IsAny<string>())).Returns("hash");
            EncrypterMock.Setup(x => x.GetHash(It.IsAny<string>(), It.IsAny<string>())).Returns("salt");
            MapperMock = new Mock<IMapper>();
            RoleRepositoryMock = new Mock<IRoleRepository>();       
            UserService = new UserService(UserRepositoryMock.Object, MapperMock.Object, EncrypterMock.Object, RoleRepositoryMock.Object);
        }

        [Fact]
        public async Task register_async_should_invoke_add_async_on_repository()
        {
            await UserService.RegisterAsync("mock", "mock@emailcom", "secret", "User");

            UserRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task when_calling_get_all_async_it_should_invoke_user_repository_get_all_async()
        {
            await UserService.GetAllAsync();
            var role = new Role("User");
            var users = new List<User>();

            for (int i = 0; i < 5; i++)
            {
                users.Add(new User($"user{i}", "user{i}@email.com", "secret", "salt", role));
            }

            UserRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(users);

            UserRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task when_calling_get_async_and_user_exists_it_should_invoke_user_repository_get_async()
        {
            await UserService.GetAsync("user1");
            var role = new Role("User");

            var user = new User("user1", "user1@email.com", "secret", "salt", role);

            UserRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(user);

            UserRepositoryMock.Verify(x => x.GetAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task when_calling_get_async_and_user_does_not_exist_it_should_invoke_user_user_repository_get_async()
        {
            await UserService.GetAsync("user1");

            UserRepositoryMock.Setup(x => x.GetAsync("user1")).ReturnsAsync(() => null);

            UserRepositoryMock.Verify(x => x.GetAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task when_calling_delete_async_and_user_exist_it_should_invoke_user_repository_delete_async()
        {
            await UserService.GetAsync("user1");

            var role = new Role("User");

            var user = new User("user1", "user1@email.com", "secret", "salt", role);

            UserRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(user);

            await UserService.DeleteAsync(user.Username);
            UserRepositoryMock.Verify(x => x.RemoveAsync(It.IsAny<User>()), Times.Once);
        }


    }
}
