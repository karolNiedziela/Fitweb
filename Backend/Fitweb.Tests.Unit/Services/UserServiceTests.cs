using AutoMapper;
using Backend.Core.Entities;
using Backend.Core.Repositories;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Mappers;
using Backend.Infrastructure.Repositories;
using Backend.Infrastructure.Services;
using NSubstitute;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Fitweb.Tests.Unit.Services
{
    public class UserServiceTests : IClassFixture<FitwebSeedDataFixture>
    { 
        private readonly IUserRepository _userRepository;
        private readonly IUserService _sut; // system under testing
        private readonly IMapper _mapper;
        private readonly IPasswordHandler _passwordHandler;
        FitwebSeedDataFixture _fixture;

        public UserServiceTests(FitwebSeedDataFixture fixture)
        {
            _fixture = fixture;
            _userRepository = Substitute.For<IUserRepository>();
            _mapper = Substitute.For<IMapper>();
            _passwordHandler = Substitute.For<IPasswordHandler>();
            _mapper = AutoMapperConfig.Initialize();
            _sut = new UserService(_mapper, _passwordHandler, _userRepository);      
        }


        [Fact]
        public async Task GetAsyncById_ShouldReturnUserDto()
        {
            // Arrange
            var user = new User("testUser", "test@email.com", "testSecret")
            {
                Id = 1
            };
          
            user.UserRoles.Add(new UserRole
            {
                User = user,
                Role = Role.GetRole("User")
            });

            _userRepository.GetAsync(user.Id).Returns(user);

            // Act
            var dto = await _sut.GetAsync(user.Id);

            // Assert
            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(user.Id);
            dto.Username.ShouldBe(user.Username);
            dto.Email.ShouldBe(user.Email);
            dto.Role.ShouldBe(user.UserRoles.Select(ur => ur.Role.Name.ToString()).FirstOrDefault());
            await _userRepository.Received(1).GetAsync(user.Id);
        }

        [Fact]
        public async Task GetAsyncByUsername_ShouldReturnUserDto()
        {
            var user = new User("testUser", "test@email.com", "testSecret");

            user.UserRoles.Add(new UserRole
            {
                User = user,
                Role = Role.GetRole("User")
            });

            _userRepository.GetAsync(user.Username).Returns(user);

            // Act
            var dto = await _sut.GetAsync(user.Username);

            // Assert
            dto.ShouldNotBeNull();
            dto.ShouldBeOfType<UserDto>();
            dto.Id.ShouldBe(user.Id);
            dto.Username.ShouldBe(user.Username);
            dto.Email.ShouldBe(user.Email);
            dto.Role.ShouldBe(user.UserRoles.Select(ur => ur.Role.Name.ToString()).FirstOrDefault());
            await _userRepository.Received(1).GetAsync(user.Username);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnIEnumerableUsersDto()
        {
            // Arrange
            var users = new List<User>
            {
                new User("testUser", "test@email.com", "testSecret"),
                new User("testUser2", "test2@email.com", "testSecret")
            };

            foreach (var user in users)
            {
                user.UserRoles.Add(new UserRole
                {
                    User = user,
                    Role = Role.GetRole("User")
                });
            }

            _userRepository.GetAllAsync().Returns(users);

            // Act

            var dto = await _sut.GetAllAsync();

            // Assert
            dto.ShouldNotBeNull();
            dto.ShouldNotBeOfType<IEnumerable<UserDto>>();
            dto.Count().ShouldBe(2);

            await _userRepository.Received(1).GetAllAsync();
        }

        [Fact]
        public async Task RegisterAsync_ShouldAddNewUser()
        {
            // Arrange 

            // Act
            await _sut.RegisterAsync("testUser", "test@email.com", "testSecret");

            // Assert
            await _userRepository.Received(1).AddAsync(Arg.Any<User>());
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrowException_WhenUserNameIsNotUnique()
        {
            var userRepository = Substitute.For<UserRepository>(_fixture.FitwebContext);

            var userService = Substitute.For<UserService>(_mapper, _passwordHandler, userRepository);

            var exception = await Record.ExceptionAsync(() =>  
                userService.RegisterAsync("user2", "karol@email.com", "secret"));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.ShouldBeOfType(typeof(ServiceException), $"User with 'user2' already exists.");
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrowException_WhenEmailIsNotUnique()
        {   
            // Arrange
            var userRepository = Substitute.For<UserRepository>(_fixture.FitwebContext);
            var userService = Substitute.For<UserService>(_mapper, _passwordHandler, userRepository);

            // Act
            var exception = await Record.ExceptionAsync(() =>
                userService.RegisterAsync("user5", "user1@email.com", "secret"));

            // Assert
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.ShouldBeOfType(typeof(ServiceException), $"User with 'user1@email.com' already exists.");
        }

        [Fact]
        public async Task LoginAsync_ShouldThrowInvalidCredential_WhenUserDoesNotExist()
        {
            // Arrange

            // Act
            var exception = await Record.ExceptionAsync(() => _sut.LoginAsync("karol", "testSecret"));

            // Assert
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.ShouldBeOfType(typeof(ServiceException), "Invalid credentials");
            exception.StackTrace.Length.Equals(2);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteUser_WhenUserExists()
        {
            // Arrange
            var userRepository = Substitute.For<UserRepository>(_fixture.FitwebContext);
            var userService = Substitute.For<UserService>(_mapper, _passwordHandler, userRepository);

            // Act
            await userService.DeleteAsync(1);

            // Assert
            var user = await userRepository.GetAsync(1);
            user.ShouldBeNull();
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange

            // Act
            var exception = await Record.ExceptionAsync(() => _sut.DeleteAsync(1));

            // Assert
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.ShouldBeOfType(typeof(ServiceException), $"User with id: 1 was not found.");
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenUserDoestNotExist()
        {
            // Arrange

            // Act
            var exception = await Record.ExceptionAsync(() => _sut.UpdateAsync(1, "karol", "karol@email.com", "testSecret"));

            // Assert
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.ShouldBeOfType(typeof(ServiceException), $"User with id: 1 was not found.");
        }
    }
}
