using AutoMapper;
using Backend.Core.Entities;
using Backend.Infrastructure.DTO;
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
    public class ExerciseServiceTests : IClassFixture<FitwebFixture>
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IMapper _mapper;
        private readonly IExerciseService _sut;
        private readonly FitwebFixture _fixture;

        public ExerciseServiceTests(FitwebFixture fixture)
        {
            _fixture = fixture;
            _exerciseRepository = Substitute.For<IExerciseRepository>();
            _mapper = Substitute.For<IMapper>();
            _mapper = AutoMapperConfig.Initialize();
            _sut = new ExerciseService(_exerciseRepository, _mapper);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetAsyncById_ShouldReturnExerciseDto(int id)
        {
            var exercise = _fixture.FitwebContext.Exercises.SingleOrDefault(e => e.Id == id);
            _exerciseRepository.GetAsync(id).Returns(exercise);

            var dto = await _sut.GetAsync(id);

            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(exercise.Id);
            dto.ShouldBeOfType(typeof(ExerciseDto));
            await _exerciseRepository.Received(1).GetAsync(id);
        }

        [Theory]
        [InlineData("exercise1")]
        [InlineData("exercise2")]
        public async Task GetAsyncByName_ShouldReturnExerciseDto(string name)
        {
            var exercise = _fixture.FitwebContext.Exercises.SingleOrDefault(e => e.Name == name);
            _exerciseRepository.GetAsync(name).Returns(exercise);

            var dto = await _sut.GetAsync(name);

            dto.ShouldNotBeNull();
            dto.Name.ShouldBe(exercise.Name);
            dto.ShouldBeOfType(typeof(ExerciseDto));
            await _exerciseRepository.Received(1).GetAsync(exercise.Name);
        }
        
        [Theory]
        [InlineData("product1", "Meat")]
        [InlineData("product1", null)]
        [InlineData(null, null)]
        public async Task GetAllAsync_ShouldReturnIEnumerableExerciseDto(string name, string category)
        {
            var exercises = _fixture.FitwebContext.Exercises.ToList();
            var pagedList = Substitute.For<PagedList<Exercise>>(exercises, 10, 10, 10);
            var paginationQuery = Substitute.For<PaginationQuery>();
            _exerciseRepository.GetAllAsync(name, category, paginationQuery).Returns(pagedList);


            var dto = await _sut.GetAllAsync(name, category, paginationQuery);

            dto.ShouldNotBeNull();
            dto.ShouldBeOfType(typeof(PagedList<ExerciseDto>));
            dto.Count().ShouldBe(exercises.Count());
        }
        
        [Fact]
        public async Task AddAsync_ShouldAddNewExercise()
        {
            await _sut.AddAsync("chest push", "Chest");

            await _exerciseRepository.Received(1).AddAsync(Arg.Any<Exercise>());
        }
        
        [Theory]
        [InlineData("exercise1")]
        [InlineData("exercise2")]
        public async Task AddAsync_ShouldThrowException_WhenExerciseExists(string name)
        {
            var exercise = _fixture.FitwebContext.Exercises.SingleOrDefault(e => e.Name == name);
            _exerciseRepository.GetAsync(name).Returns(exercise);

            var exception = await Record.ExceptionAsync(() => _sut.AddAsync(name, "Chest"));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.Message.ShouldBe($"Exercise with name: '{name}' already exists.");
        }
        
        [Fact]
        public async Task DeleteAsync_ShouldDeleteExercise_WhenExerciseExists()
        {
            var exercise = _fixture.FitwebContext.Exercises.SingleOrDefault(e => e.Id == 1);
            _exerciseRepository.GetAsync(exercise.Id).Returns(exercise);

            await _sut.DeleteAsync(exercise.Id);

            await _exerciseRepository.Received(1).DeleteAsync(Arg.Is(exercise));
        }
        
        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenExerciseDoesNotExist()
        {
            var exception = await Record.ExceptionAsync(() => _sut.DeleteAsync(1));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.ShouldBeOfType(typeof(ServiceException), "Exercise with id: 1 was not found.");

        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateExercise_WhenExerciseExistsAndDataIsValid()
        {
            var exercise = _fixture.FitwebContext.Exercises.SingleOrDefault(e => e.Id == 1);
            _exerciseRepository.GetAsync(exercise.Id).Returns(exercise);

            await _sut.UpdateAsync(1, "someName", "Chest");

            await _exerciseRepository.Received(1).UpdateAsync(Arg.Any<Exercise>());
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenExerciseDoesNotExist()
        {
            var exception = await Record.ExceptionAsync(() => _sut.UpdateAsync(1, "someName", "Chest"));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.ShouldBeOfType(typeof(ServiceException), "Exercise with id: 1 was not found.");
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenExerciseExistsButNameIsNotUnique()
        {
            var exercise = _fixture.FitwebContext.Exercises.SingleOrDefault(e => e.Id == 1);
            _exerciseRepository.GetAsync(exercise.Id).Returns(exercise);

            _exerciseRepository.AnyAsync(e => e.Name == exercise.Name).ReturnsForAnyArgs(true);

            var exception = await Record.ExceptionAsync(() => _sut.UpdateAsync(1, "exercise2", "somePart"));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.ShouldBeOfType(typeof(ServiceException), $"Exercise with 'exercise2' already exists.");
        }
    }
}
