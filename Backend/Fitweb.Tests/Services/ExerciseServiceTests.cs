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
    public class ExerciseServiceTests
    {
        private readonly Mock<IExerciseRepository> _exerciseRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ExerciseService _exerciseService;

        public ExerciseServiceTests()
        {
            _exerciseRepositoryMock = new Mock<IExerciseRepository>();
            _mapperMock = new Mock<IMapper>();
            _exerciseService = new ExerciseService(_exerciseRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task when_calling_get_async_and_exercise_exists_it_should_invoke_exercise_repository_get_async()
        {
            await _exerciseService.GetAsync("exercise1");

            var exercise = new Exercise("chest", "exercise1");

            _exerciseRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(exercise);

            _exerciseRepositoryMock.Verify(x => x.GetAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task add_async_should_invoke_add_async_on_exercise_repository()
        {

            await _exerciseService.AddAsync("chest", "product1");

            _exerciseRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Exercise>()), Times.Once);
        }

        [Fact]
        public async Task when_calling_get_async_and_exercise_does_not_exist_it_should_invoke_exercise_repository_get_async()
        {
            await _exerciseService.GetAsync("exercise1");

            _exerciseRepositoryMock.Setup(x => x.GetAsync("exercise1")).ReturnsAsync(() => null);

            _exerciseRepositoryMock.Verify(x => x.GetAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task when_calling_update_async_and_exercise_exist_it_should_invoke_exercise_repository_update_async()
        {
            await _exerciseService.GetAsync("exercise1");

            var exercise = new Exercise("chest", "exercise1");

            _exerciseRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(exercise);

            await _exerciseService.UpdateAsync(exercise.PartOfBody, exercise.Name);
            _exerciseRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Exercise>()), Times.Once);
        }

        [Fact]
        public async Task when_calling_delete_async_and_exercise_exist_it_should_invoke_exercise_repository_delete_async()
        {
            await _exerciseService.GetAsync("exercise1");

            var exercise = new Exercise("chest", "exercise1");

            _exerciseRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(exercise);

            await _exerciseService.DeleteAsync(exercise.Name);
            _exerciseRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Exercise>()), Times.Once);
        }
    }
}
