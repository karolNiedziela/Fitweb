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
    public class AthleteExerciseServiceTests : IClassFixture<FitwebFixture>
    {
        private readonly IAthleteRepository _athleteRepository;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly FitwebFixture _fixture;
        private readonly IAthleteExerciseService _sut;

        public AthleteExerciseServiceTests(FitwebFixture fixture)
        {
            _fixture = fixture;
            _athleteRepository = Substitute.For<IAthleteRepository>();
            _exerciseRepository = Substitute.For<IExerciseRepository>();
            _sut = new AthleteExerciseService(_athleteRepository, _exerciseRepository);
        }

        [Theory]
        [InlineData(1, 2, 100, 4, 12, "Monday")]
        [InlineData(1, 2, 15, 3, 10, "Sunday")]
        public async Task AddAsync_ShouldAddExercise_WhenAthleteExistsAndExerciseExistsAndDataIsValid(int userId, int exerciseId,
            double weight, int numberOfSets, int numberOfReps, string dayName)
        {
            var athlete = _fixture.FitwebContext.Athletes.Where(a => a.UserId == userId)
                                                .Include(a => a.AthleteExercises)
                                                    .ThenInclude(ae => ae.Day)
                                                .Include(a => a.AthleteExercises.Where(ae => ae.ExerciseId == exerciseId
                                                    && ae.DayId == Day.GetDay(dayName).Id && 
                                                    ae.DateUpdated.ToShortDateString() == DateTime.Today.ToShortDateString()))
                                                    .ThenInclude(ap => ap.Exercise)
                                                    .ThenInclude(e => e.PartOfBody)
                                                .AsNoTracking()
                                                .SingleOrDefault();

            var exercise = _fixture.FitwebContext.Exercises.SingleOrDefault(e => e.Id == exerciseId);

            _athleteRepository.FindByCondition(Arg.Any<Expression<Func<Athlete, bool>>>(),
                Arg.Any<Func<IQueryable<Athlete>, IIncludableQueryable<Athlete, object>>>()).Returns(athlete);

            _exerciseRepository.GetAsync(exerciseId).Returns(exercise);


            await _sut.AddAsync(userId, exerciseId, weight, numberOfSets, numberOfReps, dayName);

            await _athleteRepository.Received(1).UpdateAsync(athlete);
        }
         
        [Fact]
        public async Task AddAsync_ShouldThrowException_WhenAthleteDoesNotExist()
        {
            var exception = await Record.ExceptionAsync(() => _sut.AddAsync(1, 2, 15, 3, 10, "Sunday"));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            ((ServiceException)exception).Code.ShouldBe(Infrastructure.Exceptions.ErrorCodes.AthleteNotFound);
            exception.Message.ShouldBe($"Athlete with user id: {1} was not found.");
        }

        [Theory]
        [InlineData(1, 7, 50, 4, 12, "Tuesday")]
        public async Task AddAsync_ShouldThrowException_WhenAthleteExistsAndExerciseDoesNotExist(int userId, int exerciseId,
            double weight, int numberOfSets, int numberOfReps, string dayName)
        {
            var athlete = _fixture.FitwebContext.Athletes.Where(a => a.UserId == userId)
                                               .Include(a => a.AthleteExercises)
                                                   .ThenInclude(ae => ae.Day)
                                               .Include(a => a.AthleteExercises.Where(ae => ae.ExerciseId == exerciseId
                                                   && ae.DayId == Day.GetDay(dayName).Id &&
                                                   ae.DateUpdated.ToShortDateString() == DateTime.Today.ToShortDateString()))
                                                   .ThenInclude(ap => ap.Exercise)
                                                   .ThenInclude(e => e.PartOfBody)
                                               .AsNoTracking()
                                               .SingleOrDefault();

            _athleteRepository.FindByCondition(Arg.Any<Expression<Func<Athlete, bool>>>(),
               Arg.Any<Func<IQueryable<Athlete>, IIncludableQueryable<Athlete, object>>>()).Returns(athlete);

            var exception = await Record.ExceptionAsync(() => _sut.AddAsync(userId, exerciseId, weight, numberOfSets, 
                numberOfReps, "Monday"));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            ((ServiceException)exception).Code.ShouldBe(Infrastructure.Exceptions.ErrorCodes.ObjectNotFound);
            exception.Message.ShouldBe($"Exercise with id: {7} was not found.");
        }

        [Theory]
        [InlineData(1, 2, -30, 4, 12, "Monday")]
        public async Task AddAsync_ShouldThrowException_WhenAthleteExistsAndExerciseExistsButWeightIsNegative(int userId, int exerciseId,
            double weight, int numberOfSets, int numberOfReps, string dayName)
        {
            var athlete = _fixture.FitwebContext.Athletes.Where(a => a.UserId == userId)
                                                .Include(a => a.AthleteExercises)
                                                    .ThenInclude(ae => ae.Day)
                                                .Include(a => a.AthleteExercises.Where(ae => ae.ExerciseId == exerciseId
                                                    && ae.DayId == Day.GetDay(dayName).Id &&
                                                    ae.DateUpdated.ToShortDateString() == DateTime.Today.ToShortDateString()))
                                                    .ThenInclude(ap => ap.Exercise)
                                                    .ThenInclude(e => e.PartOfBody)
                                                .AsNoTracking()
                                                .SingleOrDefault();

            var exercise = _fixture.FitwebContext.Exercises.SingleOrDefault(e => e.Id == exerciseId);

            _athleteRepository.FindByCondition(Arg.Any<Expression<Func<Athlete, bool>>>(),
                Arg.Any<Func<IQueryable<Athlete>, IIncludableQueryable<Athlete, object>>>()).Returns(athlete);

            _exerciseRepository.GetAsync(exerciseId).Returns(exercise);

            var exception = await Record.ExceptionAsync(() => _sut.AddAsync(userId, exerciseId, weight, 
                numberOfSets, numberOfReps, dayName));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(DomainException));
            ((DomainException)exception).Code.ShouldBe(Core.Exceptions.ErrorCodes.InvalidValue);
            exception.Message.ShouldBe("Weight cannot be less than or equal to 0.");
        }

        [Theory]
        [InlineData(1, 2, 30, -4, 12, "Monday")]
        public async Task AddAsync_ShouldThrowException_WhenAthleteExistsAndExerciseExistsButNumberOfSetsIsNegative(int userId, int exerciseId,
           double weight, int numberOfSets, int numberOfReps, string dayName)
        {
            var athlete = _fixture.FitwebContext.Athletes.Where(a => a.UserId == userId)
                                                .Include(a => a.AthleteExercises)
                                                    .ThenInclude(ae => ae.Day)
                                                .Include(a => a.AthleteExercises.Where(ae => ae.ExerciseId == exerciseId
                                                    && ae.DayId == Day.GetDay(dayName).Id &&
                                                    ae.DateUpdated.ToShortDateString() == DateTime.Today.ToShortDateString()))
                                                    .ThenInclude(ap => ap.Exercise)
                                                    .ThenInclude(e => e.PartOfBody)
                                                .AsNoTracking()
                                                .SingleOrDefault();

            var exercise = _fixture.FitwebContext.Exercises.SingleOrDefault(e => e.Id == exerciseId);

            _athleteRepository.FindByCondition(Arg.Any<Expression<Func<Athlete, bool>>>(),
                Arg.Any<Func<IQueryable<Athlete>, IIncludableQueryable<Athlete, object>>>()).Returns(athlete);

            _exerciseRepository.GetAsync(exerciseId).Returns(exercise);

            var exception = await Record.ExceptionAsync(() => _sut.AddAsync(userId, exerciseId, weight,
                numberOfSets, numberOfReps, dayName));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(DomainException));
            ((DomainException)exception).Code.ShouldBe(Core.Exceptions.ErrorCodes.InvalidValue);
            exception.Message.ShouldBe($"Number of sets cannot be less than or equal to 0.");
        }

        [Theory]
        [InlineData(1, 2, 30, 4, -12, "Monday")]
        public async Task AddAsync_ShouldThrowException_WhenAthleteExistsAndExerciseExistsButNumberOfRepsIsNegative(int userId, int exerciseId,
           double weight, int numberOfSets, int numberOfReps, string dayName)
        {
            var athlete = _fixture.FitwebContext.Athletes.Where(a => a.UserId == userId)
                                                .Include(a => a.AthleteExercises)
                                                    .ThenInclude(ae => ae.Day)
                                                .Include(a => a.AthleteExercises.Where(ae => ae.ExerciseId == exerciseId
                                                    && ae.DayId == Day.GetDay(dayName).Id &&
                                                    ae.DateUpdated.ToShortDateString() == DateTime.Today.ToShortDateString()))
                                                    .ThenInclude(ap => ap.Exercise)
                                                    .ThenInclude(e => e.PartOfBody)
                                                .AsNoTracking()
                                                .SingleOrDefault();

            var exercise = _fixture.FitwebContext.Exercises.SingleOrDefault(e => e.Id == exerciseId);

            _athleteRepository.FindByCondition(Arg.Any<Expression<Func<Athlete, bool>>>(),
                Arg.Any<Func<IQueryable<Athlete>, IIncludableQueryable<Athlete, object>>>()).Returns(athlete);

            _exerciseRepository.GetAsync(exerciseId).Returns(exercise);

            var exception = await Record.ExceptionAsync(() => _sut.AddAsync(userId, exerciseId, weight,
                numberOfSets, numberOfReps, dayName));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(DomainException));
            ((DomainException)exception).Code.ShouldBe(Core.Exceptions.ErrorCodes.InvalidValue);
            exception.Message.ShouldBe($"Number of reps cannot be less than or equal to 0.");
        }
    }
}
