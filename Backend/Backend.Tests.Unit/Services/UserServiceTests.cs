using AutoMapper;
using Backend.Core.Entities;
using Backend.Core.Repositories;
using Backend.Infrastructure.Auth;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Mappers;
using Backend.Infrastructure.Repositories;
using Backend.Infrastructure.Services;
using Backend.Tests.Unit.Fixtures;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Unit.Services
{
    public class UserServiceTests : IClassFixture<FitwebFixture>
    { 
        private readonly IUserRepository _userRepository;
        private readonly IUserService _sut; // system under testing
        private readonly IMapper _mapper;
        private readonly IPasswordHandler _passwordHandler;
        private readonly FitwebFixture _fixture;

        public UserServiceTests(FitwebFixture fixture)
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
            var user = _fixture.FitwebContext.Users.SingleOrDefault(u => u.Id == 1);
             _userRepository.GetAsync(1).Returns(user);

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
            var user = _fixture.FitwebContext.Users.SingleOrDefault(u => u.Username == "user1");
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
            var users = _fixture.FitwebContext.Users.ToList();
            _userRepository.GetAllAsync().Returns(users);

            _userRepository.GetAllAsync().Returns(users);

            // Act

            var dto = await _sut.GetAllAsync();

            // Assert
            dto.ShouldNotBeNull();
            dto.ShouldNotBeOfType<IEnumerable<UserDto>>();
            dto.Count().ShouldBe(3);

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
            _userRepository.AnyAsync(u => u.Username == "user1").ReturnsForAnyArgs(true);

            var exception = await Record.ExceptionAsync(() =>  
                _sut.RegisterAsync("user1", "user1@email.com", "secret"));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.ShouldBeOfType(typeof(ServiceException), $"User with 'user2' already exists.");
            await _userRepository.Received(1).AnyAsync(Arg.Any<Expression<Func<User, bool>>>());
        }
       
        [Fact]
        public async Task RegisterAsync_ShouldThrowException_WhenEmailIsNotUnique()
        {
            // Arrange
            _userRepository.AnyAsync(u => u.Email == "user1@email.com").ReturnsForAnyArgs(true);

            // Act
            var exception = await Record.ExceptionAsync(() =>
                _sut.RegisterAsync("user5", "user1@email.com", "secret"));

            // Assert
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.ShouldBeOfType(typeof(ServiceException), $"User with 'user1@email.com' already exists.");
        }

        [Fact]
        public async Task LoginAsync_ShouldThrowException_WhenUserDoesNotExist()
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
        public async Task LoginAsync_ShouldThrowException_WhenPasswordIsNotValid()
        {
            // Arrange
            var user = _fixture.FitwebContext.Users.SingleOrDefault(u => u.Id == 1);
            _userRepository.GetAsync(user.Id).Returns(user);
            _passwordHandler.IsValid(user.Password, "someRandomPassword").ReturnsForAnyArgs(false);

            // Act
            var exception = await Record.ExceptionAsync(() => _sut.LoginAsync(user.Username, user.Password));

            // Arrange
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.ShouldBeOfType(typeof(ServiceException), "Invalid Credentials");
        }


        [Fact]
        public async Task DeleteAsync_ShouldDeleteUser_WhenUserExists()
        {
            // Arrange
            var user = _fixture.FitwebContext.Users.SingleOrDefault(u => u.Id == 1);
            _userRepository.GetAsync(user.Id).Returns(user);

            // Act
            await _sut.DeleteAsync(1);

            // Assert
            await _userRepository.Received(1).DeleteAsync(Arg.Is(user));
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
            await _userRepository.DidNotReceive().DeleteAsync(Arg.Any<User>());

        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateUser_WhenUserExistsAndDataIsValid()
        {
            // Arrange
            var user = _fixture.FitwebContext.Users.SingleOrDefault(u => u.Id == 1);
            _userRepository.GetAsync(user.Id).Returns(user);

            // Act
            await _sut.UpdateAsync(user.Id, user.Username, user.Email, user.Password);

            // Assert
            await _userRepository.Received(1).UpdateAsync(Arg.Any<User>());
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenUserExistsButUsernameIsNotUnique()
        {
            // Arrange
            var user = _fixture.FitwebContext.Users.SingleOrDefault(u => u.Id == 1);
            _userRepository.GetAsync(user.Id).Returns(user);

            _userRepository.AnyAsync(u => u.Username == "user2").ReturnsForAnyArgs(true);

            // Act
            var exception = await Record.ExceptionAsync(() =>  
                _sut.UpdateAsync(1, "user2", "user10@email.com", "secret"));

            // Assert
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.ShouldBeOfType(typeof(ServiceException), $"User with 'user2' already exists.");
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenUserExistsButEmailIsNotUnique()
        {
            // Arrange
            var user = _fixture.FitwebContext.Users.SingleOrDefault(u => u.Id == 1);
            _userRepository.GetAsync(user.Id).Returns(user);

            _userRepository.AnyAsync(u => u.Email == "user2@email.com").ReturnsForAnyArgs(true);

            // Act
            var exception = await Record.ExceptionAsync(() => 
                _sut.UpdateAsync(1, "user10", "user2@email.com", "secret"));

            // Assert
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.ShouldBeOfType(typeof(ServiceException), $"User with 'user2@email.com' already exists.");
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange

            // Act
            var exception = await Record.ExceptionAsync(() => _sut.UpdateAsync(1, "karol", "karol@email.com", "testSecret"));

            // Assert
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.ShouldBeOfType(typeof(ServiceException), $"User with id: 1 was not found.");
            await _userRepository.DidNotReceive().UpdateAsync(Arg.Any<User>());
        }
    }
}
