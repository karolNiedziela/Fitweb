using AutoMapper;
using Backend.Core.Entities;
using Backend.Core.Exceptions;
using Backend.Core.Factories;
using Backend.Core.Repositories;
using Backend.Infrastructure.Auth;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Mappers;
using Backend.Infrastructure.Repositories;
using Backend.Infrastructure.Services;
using Backend.Tests.Unit.Fixtures;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;
        private readonly IUserService _sut; // system under testing
        private readonly FitwebFixture _fixture;

        public UserServiceTests(FitwebFixture fixture)
        {
            _fixture = fixture;
            _userRepository = Substitute.For<IUserRepository>();
            _mapper = Substitute.For<IMapper>();
            _mapper = AutoMapperConfig.Initialize();
            _sut = new UserService(_mapper, _userRepository);
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
            dto.UserName.ShouldBe(user.UserName);
            dto.Email.ShouldBe(user.Email);
            dto.Role.ShouldBe(user.UserRoles.Select(ur => ur.Role.Name.ToString()).FirstOrDefault());
            await _userRepository.Received(1).GetAsync(user.Id);
        }

        [Theory]
        [InlineData("user1")]
        [InlineData("user2")]
        public async Task GetAsyncByUsername_ShouldReturnUserDto(string username)
        {
            var user = _fixture.FitwebContext.Users.SingleOrDefault(u => u.UserName == username);
            _userRepository.GetByUsernameAsync(username).Returns(user);

            // Act
            var dto = await _sut.GetByUsernameAsync(user.UserName);

            // Assert
            dto.ShouldNotBeNull();
            dto.ShouldBeOfType<UserDto>();
            dto.Id.ShouldBe(user.Id);
            dto.UserName.ShouldBe(user.UserName);
            dto.Email.ShouldBe(user.Email);
            dto.Role.ShouldBe(user.UserRoles.Select(ur => ur.Role.Name.ToString()).FirstOrDefault());
            await _userRepository.Received(1).GetByUsernameAsync(user.UserName);
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
    }
}
