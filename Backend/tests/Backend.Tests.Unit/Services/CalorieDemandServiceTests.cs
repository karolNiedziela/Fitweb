using AutoFixture;
using Backend.Core.Entities;
using Backend.Core.Exceptions;
using Backend.Core.Repositories;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Extensions;
using Backend.Infrastructure.Services.AthleteCaloricDemand;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Unit.Services
{
    public class CalorieDemandServiceTests
    {
        private readonly IAthleteRepository _athleteRepository;
        private readonly IFixture _fixture;
        private readonly CalorieDemandService _sut;

        public CalorieDemandServiceTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _athleteRepository = Substitute.For<IAthleteRepository>();
            _sut = new CalorieDemandService(_athleteRepository);
        }

        [Theory]
        [InlineData(5, 2000, 150, 300, 60)]
        [InlineData(5, 3000, 20, 440, 80)]
        public async Task SetDemandAsync_ShouldSetCalorieDemand_WhenDataIsValid(int userId, double totalCalories, 
            double proteins, double carbohydrates, double fats)
        {
            var athlete = _fixture.Build<Athlete>()
                .With(a => a.UserId, userId)
                .Without(a => a.AthleteExercises)
                .Without(a => a.AthleteProducts)
                .Without(a => a.CaloricDemand)
                .Create();

            _athleteRepository.GetOrFailAsync(userId).Returns(athlete);

            await _sut.SetDemandAsync(userId, totalCalories, proteins, carbohydrates, fats);

            await _athleteRepository.Received(1).UpdateAsync(Arg.Is<Athlete>(a =>
            a.UserId == userId &&
            a.CaloricDemand.TotalCalories == totalCalories &&
            a.CaloricDemand.Proteins == proteins &&
            a.CaloricDemand.Carbohydrates == carbohydrates &&
            a.CaloricDemand.Fats == fats
            ));
        }

        [Fact]
        public async Task SetDemandAsync_ShouldThrowException_WhenAthleteDoesNotExist()
        {
            var userId = 5;
            var totalCalories = 2500;
            var proteins = 200;
            var carbohydrates = 300;
            var fats = 50;

            var exception = await Record.ExceptionAsync(() => _sut.SetDemandAsync(userId, totalCalories, proteins,
                carbohydrates, fats));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(AthleteNotFoundException));
            exception.Message.ShouldBe($"Athlete with user id: '{userId}' was not found.");
        }

        [Fact]
        public async Task SetDemandAsync_ShouldThrowException_WhenTotalCaloriesIsInvalid()
        {
            var userId = 5;
            var totalCalories = -500;
            var proteins = 200;
            var carbohydrates = 300;
            var fats = 50;

            var athlete = _fixture.Build<Athlete>()
                .With(a => a.UserId, userId)
                .Without(a => a.AthleteExercises)
                .Without(a => a.AthleteProducts)
                .Without(a => a.CaloricDemand)
            .Create();

            _athleteRepository.GetOrFailAsync(userId).Returns(athlete);

            var exception = await Record.ExceptionAsync(() => _sut.SetDemandAsync(userId, totalCalories, proteins,
                carbohydrates, fats));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(InvalidTotalCaloriesException));
            exception.Message.ShouldBe("Total calories cannot be less than or equal to 0.");
        }

        [Fact]
        public async Task SetDemandAsync_ShouldThrowException_WhenProteinsAreInvalid()
        {
            var userId = 5;
            var totalCalories = 2500;
            var proteins = -200;
            var carbohydrates = 300;
            var fats = 50;

            var athlete = _fixture.Build<Athlete>()
               .With(a => a.UserId, userId)
               .Without(a => a.AthleteExercises)
               .Without(a => a.AthleteProducts)
               .Without(a => a.CaloricDemand)
           .Create();

            _athleteRepository.GetOrFailAsync(userId).Returns(athlete);

            var exception = await Record.ExceptionAsync(() => _sut.SetDemandAsync(userId, totalCalories, proteins,
                carbohydrates, fats));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(InvalidProteinsException));
            exception.Message.ShouldBe("Proteins cannot be less than 0.");
        }

        [Fact]
        public async Task SetDemandAsync_ShouldThrowException_WhenCarbohydratesAreInvalid()
        {
            var userId = 5;
            var totalCalories = 2500;
            var proteins = 200;
            var carbohydrates = -300;
            var fats = 50;

            var athlete = _fixture.Build<Athlete>()
               .With(a => a.UserId, userId)
               .Without(a => a.AthleteExercises)
               .Without(a => a.AthleteProducts)
           .Create();

            _athleteRepository.GetOrFailAsync(userId).Returns(athlete);

            var exception = await Record.ExceptionAsync(() => _sut.SetDemandAsync(userId, totalCalories, proteins,
                carbohydrates, fats));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(InvalidCarbohydratesException));
            exception.Message.ShouldBe("Carbohydrates cannot be less than 0.");
        }

        [Fact]
        public async Task SetDemandAsync_ShouldThrowException_WhenFatsAreInvalid()
        {
            var userId = 5;
            var totalCalories = 2500;
            var proteins = 200;
            var carbohydrates = 300;
            var fats = -50;

            var athlete = _fixture.Build<Athlete>()
               .With(a => a.UserId, userId)
               .Without(a => a.AthleteExercises)
               .Without(a => a.AthleteProducts)
               .Without(a => a.CaloricDemand)
           .Create();

            _athleteRepository.GetOrFailAsync(userId).Returns(athlete);

            var exception = await Record.ExceptionAsync(() => _sut.SetDemandAsync(userId, totalCalories, proteins,
                carbohydrates, fats));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(InvalidFatsException));
            exception.Message.ShouldBe("Fats cannot be less than 0.");
        }

        [Theory]
        [InlineData(5, 2000, 150, 300, 60)]
        [InlineData(5, 3000, 20, 440, 80)]
        public async Task SetDemandAsync_ShouldUpdateDemandAsync_WhenDemandIsAlreadySet(int userId, double totalCalories,
            double proteins, double carbohydrates, double fats)
        {
            var newTotalCalories = totalCalories + 200;
            var newProteins = proteins + 20;

            var calorieDemand = _fixture.Build<CaloricDemand>()
                .With(cd => cd.TotalCalories, totalCalories)
                .With(cd => cd.Proteins, proteins)
                .With(cd => cd.Carbohydrates, carbohydrates)
                .With(cd => cd.Fats, fats)
                .Create();

            var athlete = _fixture.Build<Athlete>()
                .With(a => a.UserId, userId)
                .With(a => a.CaloricDemand, calorieDemand)
                .Without(a => a.AthleteExercises)
                .Without(a => a.AthleteProducts)
                .Create();

            _athleteRepository.GetAsync(userId).Returns(athlete);

            await _sut.SetDemandAsync(userId, newTotalCalories, newProteins, carbohydrates, fats);

            await _athleteRepository.Received(1).UpdateAsync(Arg.Is<Athlete>(a =>
            a.UserId == userId &&
            a.CaloricDemand.TotalCalories == newTotalCalories &&
            a.CaloricDemand.Proteins == newProteins &&
            a.CaloricDemand.Carbohydrates == carbohydrates &&
            a.CaloricDemand.Fats == fats));
        }
    }
}
