using AutoFixture;
using AutoMapper;
using Backend.Core.Entities;
using Backend.Core.Repositories;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Extensions;
using Backend.Infrastructure.Mappers;
using Backend.Infrastructure.Services;
using Backend.Infrastructure.Services.Logger;
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
    public class AthleteServiceTests
    {
        private readonly IAthleteRepository _athleteRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;
        private readonly AthleteService _sut;
        private readonly IFixture _fixture;

        public AthleteServiceTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _athleteRepository = Substitute.For<IAthleteRepository>();
            _userRepository = Substitute.For<IUserRepository>();
            _mapper = AutoMapperConfig.Initialize();
            _loggerManager = Substitute.For<ILoggerManager>();
            _sut = new AthleteService(_athleteRepository, _userRepository, _mapper, _loggerManager);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetAsync_ShouldReturnAthleteDto(int id)
        {
            var athlete = _fixture.Build<Athlete>()
                .Without(a => a.AthleteExercises)
                .Without(a => a.AthleteProducts)
                .Create();
                
            _athleteRepository.GetAsync(id).Returns(athlete);

            var dto = await _sut.GetAsync(id);

            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(athlete.Id);
            dto.UserId.ShouldBe(athlete.UserId);
            await _athleteRepository.Received(1).GetAsync(id);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(5)]
        public async Task GetAllAsync_ShouldReturnAthletesDto(int count)
        {
            var athletes = _fixture.Build<Athlete>()
               .Without(a => a.AthleteExercises)
               .Without(a => a.AthleteProducts)
               .CreateMany(count: count);
            _athleteRepository.GetAllAsync().Returns(athletes);

            var dto = await _sut.GetAllAsync();

            dto.ShouldNotBeNull();
            dto.ShouldBeOfType(typeof(List<AthleteDto>));
            dto.Count().ShouldBe(count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetProductsAsync_ShouldReturnAthleteAndAllProductsAdded(int athleteId)
        {
            var athlete = _fixture.Build<Athlete>()
               .With(a => a.Id, athleteId)
               .Without(a => a.AthleteExercises)
               .Create();

            _athleteRepository.FindByCondition(Arg.Any<Expression<Func<Athlete, bool>>>(),
            Arg.Any<Func<IQueryable<Athlete>, IIncludableQueryable<Athlete, object>>>()).Returns(athlete);

            var dto = await _sut.GetProductsAsync(athleteId, DateTime.Today);

            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(athleteId);
            dto.Products.Count.ShouldBe(athlete.AthleteProducts.Count);

            var athleteProduct = athlete.AthleteProducts.FirstOrDefault();

            dto.Products[0].Weight.ShouldBe(athleteProduct.Weight);

        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 2)]
        public async Task GetProductAsync_ShouldReturnChosenProductByAthlete(int athleteId, int productId)
        {
            var product = _fixture.Build<Product>().With(e => e.Id, productId).Create();
            var athleteProducts = _fixture.Build<AthleteProduct>()
                .With(ae => ae.AthleteId, athleteId)
                .With(ae => ae.ProductId, productId)
                .With(ae => ae.Product, product)
                .CreateMany(count: 1);


            var athlete = _fixture.Build<Athlete>()
              .With(a => a.Id, athleteId)
              .With(a => a.AthleteProducts, athleteProducts.ToList())
              .Without(a => a.AthleteExercises)
              .Create();


            _athleteRepository.FindByCondition(Arg.Any<Expression<Func<Athlete, bool>>>(),
                Arg.Any<Func<IQueryable<Athlete>, IIncludableQueryable<Athlete, object>>>()).Returns(athlete);

            var dto = await _sut.GetProductAsync(athleteId, productId);

            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(athleteId);
            dto.Products.Count.ShouldBe(1);

            var athleteProduct = athlete.AthleteProducts.FirstOrDefault();

            dto.Products[0].Weight.ShouldBe(athleteProduct.Weight);
            dto.Products[0].Product.Id.ShouldBe(productId);
            dto.Products[0].Product.Name.ShouldBe(athleteProduct.Product.Name);
        }

        [Theory]
        [InlineData(1, "Monday")]
        [InlineData(2, "Monday")]
        public async Task GetExercisesAsync_ShouldReturnAthleteAndAllExercisesAdded(int athleteId, string dayName)
        {
            var athlete = _fixture.Build<Athlete>()
              .With(a => a.Id, athleteId)
              .Without(a => a.AthleteProducts)
              .Create();

            _athleteRepository.FindByCondition(Arg.Any<Expression<Func<Athlete, bool>>>(),
                Arg.Any<Func<IQueryable<Athlete>, IIncludableQueryable<Athlete, object>>>()).Returns(athlete);

            var dto = await _sut.GetExercisesAsync(athleteId, dayName);

            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(athleteId);
            dto.Exercises.Count.ShouldBe(athlete.AthleteExercises.Count);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 1)]
        public async Task GetExerciseAsync_ShouldReturnChosenExerciseByAthlete(int athleteId, int exerciseId)
        {
            var exercise = _fixture.Build<Exercise>().With(e => e.Id, exerciseId).Create();
            var athleteExercises = _fixture.Build<AthleteExercise>()
                .With(ae => ae.AthleteId, athleteId)
                .With(ae => ae.ExerciseId, exerciseId)
                .With(ae => ae.Exercise, exercise)
                .CreateMany(count: 1);

            var athlete = _fixture.Build<Athlete>()
                .With(a => a.AthleteExercises, athleteExercises.ToList())
                .With(a => a.Id, athleteId)
                .Without(a => a.AthleteProducts)
                .Create();

            _athleteRepository.FindByCondition(Arg.Any<Expression<Func<Athlete, bool>>>(),
                Arg.Any<Func<IQueryable<Athlete>, IIncludableQueryable<Athlete, object>>>()).Returns(athlete);

            var dto = await _sut.GetExerciseAsync(athleteId, exerciseId);

            dto.ShouldNotBeNull();
            dto.Exercises.Count.ShouldBe(1);

            var athleteExercise = athlete.AthleteExercises.FirstOrDefault();

            dto.Exercises[0].Exercise.Id.ShouldBe(exerciseId);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateAthlete_WhenAthleteDoesNotExistAndUserExists()
        {
            var user = _fixture.Create<User>();

            _userRepository.GetOrFailAsync(user.Id).Returns(user);

            await _sut.CreateAsync(user.Id);

            await _athleteRepository.Received(1).AddAsync(Arg.Is<Athlete>(a => 
            a.UserId == user.Id));
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WhenUserDoesNotExist()
        {
            var exception = await Record.ExceptionAsync(() => _sut.CreateAsync(10));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(UserNotFoundException));
            exception.Message.ShouldBe($"User with id: '{10}' was not found.");
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WhenAthleteAlreadyExists()
        {
            var user = _fixture.Create<User>();
            var athlete = _fixture.Build<Athlete>()
                .With(a => a.UserId, user.Id)
                .Without(a => a.AthleteExercises)
                .Without(a => a.AthleteProducts)
                .Create();

            _userRepository.GetOrFailAsync(user.Id).Returns(user);
            _athleteRepository.FindByCondition(Arg.Any<Expression<Func<Athlete, bool>>>()).Returns(athlete);

            var exception = await Record.ExceptionAsync(() => _sut.CreateAsync(user.Id));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(AthleteExistsException));
            exception.Message.ShouldBe($"Athlete with user id: '{athlete.UserId}' already exists.");
            await _athleteRepository.Received(0).AddAsync(Arg.Any<Athlete>());
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteAthlete_WhenAthleteExists()
        {
            var userId = 5;
            var athlete = _fixture.Build<Athlete>()
                .With(a => a.UserId, userId)
                .Without(a => a.AthleteExercises)
                .Without(a => a.AthleteProducts)
                .Create();

            _athleteRepository.GetOrFailAsync(userId).Returns(athlete);

            await _sut.DeleteAsync(athlete.UserId);

            await _athleteRepository.Received(1).DeleteAsync(Arg.Any<Athlete>());
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenAthleteDoesNotExist()
        {
            var exception = await Record.ExceptionAsync(() => _sut.DeleteAsync(50));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(AthleteNotFoundException));
            exception.Message.ShouldBe($"Athlete with user id: '{50}' was not found.");
        }
    }
}
