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


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetAsyncById_ShouldReturnUserDto(int id)
        {
            // Arrange
            var user = _fixture.FitwebContext.Users.SingleOrDefault(u => u.Id == id);
             _userRepository.GetAsync(id).Returns(user);

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

        [Theory]
        [InlineData("user1")]
        [InlineData("user2")]
        [InlineData("user3")]
        public async Task GetAsyncByUsername_ShouldReturnUserDto(string username)
        {
            var user = _fixture.FitwebContext.Users.SingleOrDefault(u => u.Username == username);
            _userRepository.GetAsync(username).Returns(user);

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
        public async Task GetAllAsync_ShouldReturnUsersDto()
        {
            // Arrange
            var users = _fixture.FitwebContext.Users.ToList();
            _userRepository.GetAllAsync().Returns(users);

            // Act

            var dto = await _sut.GetAllAsync();

            // Assert
            dto.ShouldNotBeNull();
            dto.ShouldBeOfType(typeof(List<UserDto>));
            dto.Count().ShouldBe(3);

            await _userRepository.Received(1).GetAllAsync();
        }
    
        [Theory]
        [InlineData("testUser", "test@email.com", "testSecret")]
        public async Task RegisterAsync_ShouldAddNewUser(string username, string email, string password)
        {
            // Arrange 
            _userRepository.AnyAsync(Arg.Any<Expression<Func<User, bool>>>()).Returns(false);

            // Act
            await _sut.RegisterAsync(username, email, password);

            // Assert
            await _userRepository.Received(1).AddAsync(Arg.Any<User>());
        }

        [Theory]
        [InlineData("user1", "user1@email.com", "secret")]
        [InlineData("user2", "user2@email.com", "secret")]
        [InlineData("user3", "user3@email.com", "secret")]
        public async Task RegisterAsync_ShouldThrowException_WhenUserNameIsNotUnique(string username, 
            string email, string password)
        {
            _userRepository.AnyAsync(Arg.Any<Expression<Func<User, bool>>>()).Returns(true);

            var exception = await Record.ExceptionAsync(() =>  
                _sut.RegisterAsync(username, email, password));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.Message.ShouldBe($"User with '{username}' already exists.");
            await _userRepository.Received(1).AnyAsync(Arg.Any<Expression<Func<User, bool>>>());
        }


        // TODO: Cant figure out the problem with linq expression
        [Theory]
        [InlineData("user1", "user1@email.com", "secret")]
        [InlineData("user2", "user2@email.com", "secret")]
        [InlineData("user3", "user3@email.com", "secret")]
        public async Task RegisterAsync_ShouldThrowException_WhenEmailIsNotUnique(string username,
            string email, string password)
        {
            // Arranges
            _userRepository.AnyAsync(Arg.Any<Expression<Func<User, bool>>>()).Returns(true);

            // Act
            var exception = await Record.ExceptionAsync(() =>
                _sut.RegisterAsync(username, email, password));

            // Assert
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            //exception.Message.ShouldBe($"User with 'user1@email.com' already exists.");
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
            exception.Message.ShouldBe("Invalid credentials");
            exception.StackTrace.Length.Equals(2);
        }

        [Fact]
        public async Task LoginAsync_ShouldThrowException_WhenPasswordIsNotValid()
        {
            // Arrange
            var user = _fixture.FitwebContext.Users.SingleOrDefault(u => u.Id == 1);
            _userRepository.GetAsync(user.Id).Returns(user);
            _passwordHandler.IsValid(Arg.Is(user.Password), Arg.Any<string>()).Returns(false);

            // Act
            var exception = await Record.ExceptionAsync(() => _sut.LoginAsync(user.Username, user.Password));

            // Arrange
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.Message.ShouldBe("Invalid credentials");
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task DeleteAsync_ShouldDeleteUser_WhenUserExists(int id)
        {
            // Arrange
            var user = _fixture.FitwebContext.Users.SingleOrDefault(u => u.Id == id);
            _userRepository.GetAsync(id).Returns(user);

            // Act
            await _sut.DeleteAsync(id);

            // Assert
            await _userRepository.Received(1).DeleteAsync(Arg.Is(user));
        }


        [Theory]
        [InlineData(153)]
        [InlineData(2000)]
        public async Task DeleteAsync_ShouldThrowException_WhenUserDoesNotExist(int id)
        {
            // Arrange

            // Act
            var exception = await Record.ExceptionAsync(() => _sut.DeleteAsync(id));

            // Assert
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.Message.ShouldBe($"User with id: {id} was not found.");
            await _userRepository.DidNotReceive().DeleteAsync(Arg.Any<User>());

        }

        [Theory]
        [InlineData(1, "user1", "user1@email", "secret")]
        [InlineData(2, "user2", "user2@email", "secret")]
        public async Task UpdateAsync_ShouldUpdateUser_WhenUserExistsAndDataIsValid(int id, string username,
            string email, string password)
        {
            // Arrange
            var user = _fixture.FitwebContext.Users.SingleOrDefault(u => u.Id == id);
            _userRepository.GetAsync(id).Returns(user);

            // Act
            await _sut.UpdateAsync(id, username, email, password);

            // Assert
            await _userRepository.Received(1).UpdateAsync(Arg.Any<User>());
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenUserExistsButUsernameIsNotUnique()
        {
            // Arrange
            var user = _fixture.FitwebContext.Users.SingleOrDefault(u => u.Id == 3);
            _userRepository.GetAsync(3).Returns(user);

            _userRepository.AnyAsync(Arg.Any<Expression<Func<User, bool>>>()).Returns(true);

            // Act
            var exception = await Record.ExceptionAsync(() =>  
                _sut.UpdateAsync(3, "user1", "user1@email.com", "secret"));

            // Assert
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.Message.ShouldBe($"User with 'user1' already exists.");
        }

        // TODO: Cant figure out the problem with linq expression
        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenUserExistsButEmailIsNotUnique()
        {
            // Arrange
            var user = _fixture.FitwebContext.Users.SingleOrDefault(u => u.Id == 1);
            _userRepository.GetAsync(user.Id).Returns(user);

            _userRepository.AnyAsync(Arg.Any<Expression<Func<User, bool>>>()).ReturnsForAnyArgs(true);

            // Act
            var exception = await Record.ExceptionAsync(() => 
                _sut.UpdateAsync(1, "user1", "user1@email.com", "secret"));

            // Assert
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
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
            exception.Message.ShouldBe($"User with id: 1 was not found.");
            await _userRepository.DidNotReceive().UpdateAsync(Arg.Any<User>());
        }
    }
}
