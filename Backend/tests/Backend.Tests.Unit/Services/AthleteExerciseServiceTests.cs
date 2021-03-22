using AutoFixture;
using Backend.Core.Entities;
using Backend.Core.Exceptions;
using Backend.Core.Repositories;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Services;
using Backend.Tests.Unit.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Unit.Services
{
    public class AthleteExerciseServiceTests
    {
        private readonly IAthleteRepository _athleteRepository;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IFixture _fixture;
        private readonly AthleteExerciseService _sut;

        public AthleteExerciseServiceTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _athleteRepository = Substitute.For<IAthleteRepository>();
            _exerciseRepository = Substitute.For<IExerciseRepository>();
            _sut = new AthleteExerciseService(_athleteRepository, _exerciseRepository);
        }

        [Theory]
        [InlineData(1, 2, 100, 4, 12, "Monday")]
        [InlineData(1, 2, 15, 3, 10, "Sunday")]
        public async Task AddAsync_ShouldAddExercise_WhenAthleteExistsAndExerciseExistsAndDataIsValid(int userId, 
            int exerciseId, double weight, int numberOfSets, int numberOfReps, string dayName)
        {
            var athlete = _fixture.Build<Athlete>()
                .With(a => a.UserId, userId)
                .With(a => a.AthleteExercises, new List<AthleteExercise>())
                .With(a => a.AthleteProducts, new List<AthleteProduct>())
                .Create();

            var exercise = _fixture.Build<Exercise>()
                .With(e => e.Id, exerciseId)
                .Without(e => e.AthleteExercises)
                .Create();

            _athleteRepository.FindByCondition(Arg.Any<Expression<Func<Athlete, bool>>>(),
                Arg.Any<Func<IQueryable<Athlete>, IIncludableQueryable<Athlete, object>>>()).Returns(athlete);

            _exerciseRepository.GetAsync(exerciseId).Returns(exercise);


            await _sut.AddAsync(userId, exerciseId, weight, numberOfSets, numberOfReps, dayName);

            await _athleteRepository.Received(1).UpdateAsync(athlete);
        }

        [Fact]
        public async Task AddAsync_ShouldException_WhenAthleteDoesNotExist()
        {
            var exception = await Record.ExceptionAsync(() => _sut.AddAsync(1, 2, 15, 3, 10, "Sunday"));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(AthleteNotFoundException));
            exception.Message.ShouldBe($"Athlete with user id: '{1}' was not found.");
        }

        [Theory]
        [InlineData(1, 7, 50, 4, 12, "Tuesday")]
        public async Task AddAsync_ShouldThrowException_WhenAthleteExistsAndExerciseDoesNotExist(int userId, 
            int exerciseId, double weight, int numberOfSets, int numberOfReps, string dayName)
        {
            var athlete = _fixture.Build<Athlete>()
               .With(a => a.UserId, userId)
               .With(a => a.AthleteExercises, new List<AthleteExercise>())
               .With(a => a.AthleteProducts, new List<AthleteProduct>())
               .Create();

            _athleteRepository.FindByCondition(Arg.Any<Expression<Func<Athlete, bool>>>(),
               Arg.Any<Func<IQueryable<Athlete>, IIncludableQueryable<Athlete, object>>>()).Returns(athlete);

            var exception = await Record.ExceptionAsync(() => _sut.AddAsync(userId, exerciseId, weight, numberOfSets,
                numberOfReps, dayName));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ExerciseNotFoundException));
            exception.Message.ShouldBe($"Exercise with id: '{exerciseId}' was not found.");
        }

        [Theory]
        [InlineData(1, 2, -30, 4, 12, "Monday")]
        public async Task AddAsync_ShouldThrowException_WhenAthleteExistsAndExerciseExistsButWeightIsNegative(int userId, 
            int exerciseId, double weight, int numberOfSets, int numberOfReps, string dayName)
        {
            var athlete = _fixture.Build<Athlete>()
               .With(a => a.UserId, userId)
               .With(a => a.AthleteExercises, new List<AthleteExercise>())
               .With(a => a.AthleteProducts, new List<AthleteProduct>())
               .Create();

            var exercise = _fixture.Build<Exercise>()
                .With(e => e.Id, exerciseId)
                .Without(e => e.AthleteExercises)
                .Create();

            _athleteRepository.FindByCondition(Arg.Any<Expression<Func<Athlete, bool>>>(),
                Arg.Any<Func<IQueryable<Athlete>, IIncludableQueryable<Athlete, object>>>()).Returns(athlete);

            _exerciseRepository.GetAsync(exerciseId).Returns(exercise);

            var exception = await Record.ExceptionAsync(() => _sut.AddAsync(userId, exerciseId, weight,
                numberOfSets, numberOfReps, dayName));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(InvalidWeightException));
            exception.Message.ShouldBe("Weight cannot be less than or equal to 0.");
        }

        [Theory]
        [InlineData(1, 2, 30, -4, 12, "Monday")]
        public async Task AddAsync_ShouldThrowException_WhenAthleteExistsAndExerciseExistsButNumberOfSetsIsNegative(int userId, int exerciseId,
           double weight, int numberOfSets, int numberOfReps, string dayName)
        {
            var athlete = _fixture.Build<Athlete>()
               .With(a => a.UserId, userId)
               .With(a => a.AthleteExercises, new List<AthleteExercise>())
               .With(a => a.AthleteProducts, new List<AthleteProduct>())
               .Create();

            var exercise = _fixture.Build<Exercise>()
                .With(e => e.Id, exerciseId)
                .Without(e => e.AthleteExercises)
                .Create();

            _athleteRepository.FindByCondition(Arg.Any<Expression<Func<Athlete, bool>>>(),
                Arg.Any<Func<IQueryable<Athlete>, IIncludableQueryable<Athlete, object>>>()).Returns(athlete);

            _exerciseRepository.GetAsync(exerciseId).Returns(exercise);

            var exception = await Record.ExceptionAsync(() => _sut.AddAsync(userId, exerciseId, weight,
                numberOfSets, numberOfReps, dayName));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(InvalidNumberOfSetsException));
            exception.Message.ShouldBe($"Number of sets cannot be less than or equal to 0.");
        }

        [Theory]
        [InlineData(1, 2, 30, 4, -12, "Monday")]
        public async Task AddAsync_ShouldThrowException_WhenAthleteExistsAndExerciseExistsButNumberOfRepsIsNegative(int userId, int exerciseId,
           double weight, int numberOfSets, int numberOfReps, string dayName)
        {
            var athlete = _fixture.Build<Athlete>()
                .With(a => a.UserId, userId)
                .With(a => a.AthleteExercises, new List<AthleteExercise>())
                .With(a => a.AthleteProducts, new List<AthleteProduct>())
                .Create();

            var exercise = _fixture.Build<Exercise>()
                .With(e => e.Id, exerciseId)
                .Without(e => e.AthleteExercises)
                .Create();

            _athleteRepository.FindByCondition(Arg.Any<Expression<Func<Athlete, bool>>>(),
                Arg.Any<Func<IQueryable<Athlete>, IIncludableQueryable<Athlete, object>>>()).Returns(athlete);

            _exerciseRepository.GetAsync(exerciseId).Returns(exercise);

            var exception = await Record.ExceptionAsync(() => _sut.AddAsync(userId, exerciseId, weight,
                numberOfSets, numberOfReps, dayName));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(InvalidNumberOfRepsException));
            exception.Message.ShouldBe($"Number of reps cannot be less than or equal to 0.");
        }

        [Theory]
        [InlineData(1, 3)] 
        [InlineData(5, 10)] 
        [InlineData(10, 12)] 
        public async Task DeleteAsync_ShouldRemoveExercise_WhenAthleteExistsAndExerciseExists(int userId, int exerciseId)
        {
            var athleteExercises = _fixture.Build<AthleteExercise>()
                .With(ae => ae.ExerciseId, exerciseId)
                .With(ae => ae.AthleteId, userId)
                .CreateMany(count: 1).ToList();

            var athlete = _fixture.Build<Athlete>()
               .With(a => a.UserId, userId)
               .With(a => a.AthleteExercises, athleteExercises)
               .Without(a => a.AthleteProducts)
               .Create();


            _athleteRepository.FindByCondition(Arg.Any<Expression<Func<Athlete, bool>>>(),
                Arg.Any<Func<IQueryable<Athlete>, IIncludableQueryable<Athlete, object>>>()).Returns(athlete);

            await _sut.DeleteAsync(userId, exerciseId);

            await _athleteRepository.Received(1).UpdateAsync(Arg.Is(athlete));
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenAthleteDoesNotExist()
        {
            var userId = 5;
            var exerciseId = 10;

            var exception = await Record.ExceptionAsync(() =>  _sut.DeleteAsync(userId, exerciseId));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(AthleteNotFoundException));
            exception.Message.ShouldBe($"Athlete with user id: '{userId}' was not found.");
        }

        [Fact]
        public async Task Delete_ShouldThrowException_WhenAthleteExistsButExerciseDoesNotExist()
        {
            int userId = 5;
            int exerciseId = 3;
            var athlete = _fixture.Build<Athlete>()
               .With(a => a.UserId, userId)
               .With(a => a.AthleteExercises, new List<AthleteExercise>())
               .Without(a => a.AthleteProducts)
               .Create();

            _athleteRepository.FindByCondition(Arg.Any<Expression<Func<Athlete, bool>>>(),
                Arg.Any<Func<IQueryable<Athlete>, IIncludableQueryable<Athlete, object>>>()).Returns(athlete);

            var exception = await Record.ExceptionAsync(() => _sut.DeleteAsync(userId, exerciseId));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ExerciseForAthleteNotFoundException));
            exception.Message.ShouldBe($"Exercise with id: '{exerciseId}' for athlete " +
                $"with user id: '{userId}' was not found.");
        }
    }
}
