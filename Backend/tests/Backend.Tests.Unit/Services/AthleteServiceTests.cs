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
    public class AthleteServiceTests : IClassFixture<FitwebFixture>
    {
        private readonly IAthleteRepository _athleteRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;
        private readonly IAthleteService _sut;
        private readonly FitwebFixture _fixture;

        public AthleteServiceTests(FitwebFixture fixture)
        {
            _athleteRepository = Substitute.For<IAthleteRepository>();
            _userRepository = Substitute.For<IUserRepository>();
            _mapper = AutoMapperConfig.Initialize();
            _loggerManager = Substitute.For<ILoggerManager>();
            _fixture = fixture;
            _sut = new AthleteService(_athleteRepository, _userRepository, _mapper, _loggerManager);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetAsync_ShouldReturnAthleteDto(int id)
        {
            var athlete = _fixture.FitwebContext.Athletes.SingleOrDefault(a => a.Id == id);
            _athleteRepository.GetAsync(id).Returns(athlete);

            var dto = await _sut.GetAsync(id);

            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(athlete.Id);
            dto.UserId.ShouldBe(athlete.UserId);
            await _athleteRepository.Received(1).GetAsync(id);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAthletesDto()
        {
            var athletes = _fixture.FitwebContext.Athletes.ToList();
            _athleteRepository.GetAllAsync().Returns(athletes);

            var dto = await _sut.GetAllAsync();

            dto.ShouldNotBeNull();
            dto.ShouldBeOfType(typeof(List<AthleteDto>));
            dto.Count().ShouldBe(2);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetProductsAsync_ShouldReturnAthleteAndAllProductsAdded(int athleteId)
        {
            var athlete = _fixture.FitwebContext.Athletes.AsNoTracking().Include(a => a.AthleteProducts)
                                                            .ThenInclude(ap => ap.Product)
                                                                .ThenInclude(p => p.CategoryOfProduct)
                                                          .SingleOrDefault(a => a.Id == athleteId);

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
            var athlete = _fixture.FitwebContext.Athletes.AsNoTracking().Where(a => a.Id == athleteId).Include(a => a.AthleteProducts
                                                            .Where(ap => ap.ProductId == productId))
                                                                .ThenInclude(ap => ap.Product)
                                                                .ThenInclude(p => p.CategoryOfProduct)
                                                            .SingleOrDefault();



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
            var athlete = _fixture.FitwebContext.Athletes.AsNoTracking().Include(a => a.AthleteExercises)
                                                            .ThenInclude(ae => ae.Exercise)
                                                            .ThenInclude(e => e.PartOfBody)
                                                .SingleOrDefault(a => a.Id == athleteId);

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
            var athlete = _fixture.FitwebContext.Athletes.AsNoTracking().Where(a => a.Id == athleteId)
                                                .Include(a => a.AthleteExercises.Where(ae => ae.ExerciseId == exerciseId))
                                                    .ThenInclude(ae => ae.Exercise)
                                                    .ThenInclude(e => e.PartOfBody)
                                                .SingleOrDefault();

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
            var user = _fixture.FitwebContext.Users.SingleOrDefault(u => u.Id == 1);

            _userRepository.GetOrFailAsync(1).Returns(user);

            await _sut.CreateAsync(1);

            await _athleteRepository.Received(1).AddAsync(Arg.Any<Athlete>());
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WhenUserDoesNotExist()
        {
            var exception = await Record.ExceptionAsync(() => _sut.CreateAsync(10));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            ((ServiceException)exception).Code.ShouldBe(ErrorCodes.UserNotFound);
            exception.Message.ShouldBe($"User with id: {10} was not found.");
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WhenAthleteAlreadyExists()
        {
            var user = _fixture.FitwebContext.Users.SingleOrDefault(u => u.Id == 1);
            var athlete = _fixture.FitwebContext.Athletes.SingleOrDefault(a => a.UserId == 1);

            _userRepository.GetOrFailAsync(1).Returns(user);
            _athleteRepository.FindByCondition(Arg.Any<Expression<Func<Athlete, bool>>>()).Returns(athlete);

            var exception = await Record.ExceptionAsync(() =>  _sut.CreateAsync(1));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.Message.ShouldBe($"Athlete with user id: {1} already exists.");
            ((ServiceException)exception).Code.ShouldBe(ErrorCodes.ObjectAlreadyAdded);
            await _athleteRepository.Received(0).AddAsync(Arg.Any<Athlete>());
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteAthlete_WhenAthleteExists()
        {
            var athlete = _fixture.FitwebContext.Athletes.SingleOrDefault(a => a.Id == 1);

            _athleteRepository.GetOrFailAsync(1).Returns(athlete);

            await _sut.DeleteAsync(1);

            await _athleteRepository.Received(1).DeleteAsync(Arg.Any<Athlete>());
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenAthleteDoesNotExist()
        {
            var exception = await Record.ExceptionAsync(() => _sut.DeleteAsync(50));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            ((ServiceException)exception).Code.ShouldBe(ErrorCodes.AthleteNotFound);
            exception.Message.ShouldBe($"Athlete with user id: {50} was not found.");
        }
    }
}
