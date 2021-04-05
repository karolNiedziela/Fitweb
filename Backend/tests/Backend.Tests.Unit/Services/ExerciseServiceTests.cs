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
using AutoFixture;
using Backend.Core.Exceptions;

namespace Backend.Tests.Unit.Services
{
    public class ExerciseServiceTests
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IMapper _mapper;
        private readonly ExerciseService _sut;
        private readonly IFixture _fixture;

        public ExerciseServiceTests()
        {
            _fixture = new Fixture();
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
            var exercise = _fixture.Build<Exercise>()
                .With(e => e.Id, id)
                .Create();
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
            var exercise = _fixture.Build<Exercise>()
                .With(e => e.Name, name)
                .Create();
            _exerciseRepository.GetAsync(name).Returns(exercise);

            var dto = await _sut.GetAsync(name);

            dto.ShouldNotBeNull();
            dto.Name.ShouldBe(exercise.Name);
            dto.ShouldBeOfType(typeof(ExerciseDto));
            await _exerciseRepository.Received(1).GetAsync(exercise.Name);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        public async Task GetAllAsync_ShouldReturnPagedListOfAllExercisesDto_WhenParametersAreNull(int count)
        {
            // Arrange
            var exercises = _fixture.Build<Exercise>()
                          .CreateMany(count: count);

            var paginationQuery = new PaginationQuery();
            var page = new PagedList<Exercise>(exercises.ToList(), exercises.ToList().Count(), 1, 10);
            _exerciseRepository.GetAllAsync(null, null, paginationQuery).Returns(page);


            // Act
            var dto = await _sut.GetAllAsync(null, null, paginationQuery);

            // Assert
            dto.ShouldNotBeNull();
            dto.ShouldBeOfType(typeof(PageResultDto<ExerciseDto>));
            dto.Items.Count.ShouldBe(exercises.ToList().Count);
        }

        [Fact]
        public async Task AddAsync_ShouldAddNewExercise()
        {
            var exercise = _fixture.Build<Exercise>()
                .Create();

            await _sut.AddAsync(exercise.Name, exercise.PartOfBody.Name.ToString());

            await _exerciseRepository.Received(1).AddAsync(Arg.Any<Exercise>());
        }

        [Fact]
        public async Task AddAsync_ShouldThrowException_WheNameIsInvalid()
        {
            var name = "";
            var partOfBody = "Chest";

            var exception = await Record.ExceptionAsync(() =>  _sut.AddAsync(name, partOfBody));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(InvalidNameException));
            exception.Message.ShouldBe("Name cannot be empty.");
        }

        [Theory]
        [InlineData("exercise1")]
        [InlineData("exercise2")]
        public async Task AddAsync_ShouldThrowException_WhenExerciseExists(string name)
        {
            var exercise = _fixture.Build<Exercise>()
                .Create();
            _exerciseRepository.GetAsync(name).Returns(exercise);

            var exception = await Record.ExceptionAsync(() => _sut.AddAsync(name, exercise.PartOfBody.Name.ToString()));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(NameInUseException));
            exception.Message.ShouldBe($"Exercise with name: '{name}' already exists.");
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteExercise_WhenExerciseExists()
        {
            var exercise = _fixture.Build<Exercise>()
                .Create();
            _exerciseRepository.GetAsync(exercise.Id).Returns(exercise);

            await _sut.DeleteAsync(exercise.Id);

            await _exerciseRepository.Received(1).DeleteAsync(Arg.Is(exercise));
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenExerciseDoesNotExist()
        {
            var exception = await Record.ExceptionAsync(() => _sut.DeleteAsync(1));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ExerciseNotFoundException));
            exception.Message.ShouldBe("Exercise with id: '1' was not found.");

        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateExercise_WhenExerciseExistsAndDataIsValid()
        {
            var exercise = _fixture.Build<Exercise>()
                .Create();
            _exerciseRepository.GetAsync(exercise.Id).Returns(exercise);

            await _sut.UpdateAsync(exercise.Id, exercise.Name, exercise.PartOfBody.Name.ToString());

            await _exerciseRepository.Received(1).UpdateAsync(Arg.Is(exercise));
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenExerciseDoesNotExist()
        {
            var exception = await Record.ExceptionAsync(() => _sut.UpdateAsync(1, "someName", "Chest"));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ExerciseNotFoundException));
            exception.Message.ShouldBe("Exercise with id: '1' was not found.");
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenExerciseExistsButNameIsNotUnique()
        {
            var existing = "randomExercise";
            var exercise = _fixture.Build<Exercise>()
                .Create();
            _exerciseRepository.GetAsync(exercise.Id).Returns(exercise);

            _exerciseRepository.AnyAsync(e => e.Name == existing).ReturnsForAnyArgs(true);

            var exception = await Record.ExceptionAsync(() => _sut.UpdateAsync(exercise.Id, existing, 
                exercise.PartOfBody.Name.ToString()));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(NameInUseException));
            exception.Message.ShouldBe($"Exercise with name: '{existing}' already exists.");
        }
    }
}
