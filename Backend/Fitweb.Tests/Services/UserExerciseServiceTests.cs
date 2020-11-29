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
    public class UserExerciseServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IExerciseRepository> _exerciseRepositoryMock;
        private readonly Mock<IUserExerciseRepository> _userExerciseRepositoryMock;
        private readonly Mock<IDayRepository> _dayRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UserExerciseService _userExerciseService;

        public UserExerciseServiceTests()
        {
            _userExerciseRepositoryMock = new Mock<IUserExerciseRepository>();
            _exerciseRepositoryMock = new Mock<IExerciseRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _dayRepositoryMock = new Mock<IDayRepository>();
            _mapperMock = new Mock<IMapper>();
            _userExerciseService = new UserExerciseService(_userRepositoryMock.Object, _exerciseRepositoryMock.Object, _userExerciseRepositoryMock.Object, _dayRepositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task GetAsync_ShouldInvokeGetAsyncOnRepository()
        {
            await _userExerciseService.GetAsync(1);
            var role = new Role("User");
            var day = new Day("Monday");

            var user = new User("user1", "user1@email.com", "secret", "salt", role);
            var exercise = new Exercise("chest", "benchpress");

            var userExercise = new UserExercise(user, exercise, 100, 4, 12, day);

            _userExerciseRepositoryMock.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync(userExercise);
            _userExerciseRepositoryMock.Verify(x => x.GetAsync(It.IsAny<int>()), Times.Once);
        }
        [Fact]
        public async Task DeleteAsync_ShouldInvokeDeleteAsyncOnRepository()
        {
            var role = new Role("User");
            var day = new Day("Monday");

            var user = new User("user1", "user1@email.com", "secret", "salt", role);
            var exercise = new Exercise("chest", "benchpress");

            var userExercise = new UserExercise(user, exercise, 100, 4, 12, day);
            _userExerciseRepositoryMock.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync(userExercise);

            await _userExerciseService.DeleteAsync(1);
            _userExerciseRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<UserExercise>()), Times.Once);
        }
    }
}
