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
    public class AthleteProductServiceTests
    {
        private readonly IAthleteRepository _athleteRepository;
        private readonly IProductRepository _productRepository;
        private readonly AthleteProductService _sut;
        private readonly IFixture _fixture;

        public AthleteProductServiceTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _athleteRepository = Substitute.For<IAthleteRepository>();
            _productRepository = Substitute.For<IProductRepository>();
            _sut = new AthleteProductService(_athleteRepository, _productRepository);
        }

        [Theory]
        [InlineData(1, 3, 150)]
        [InlineData(2, 3, 500)]
        public async Task AddAsync_ShouldAddProduct_WhenAthleteExistsAndProductExistsAndWeightIsValid(int userId, int productId,
            double weight)
        {
            var product = _fixture.Build<Product>().With(p => p.Id, productId).Create();

            var athlete = _fixture.Build<Athlete>()
                .With(a => a.UserId, userId)
                .Without(a => a.AthleteExercises)
                .Create();


            _athleteRepository.FindByCondition(Arg.Any<Expression<Func<Athlete, bool>>>(),
                Arg.Any<Func<IQueryable<Athlete>, IIncludableQueryable<Athlete, object>>>()).Returns(athlete);

            _productRepository.GetAsync(productId).Returns(product);

            await _sut.AddAsync(userId, productId, weight);

            await _athleteRepository.Received(1).UpdateAsync(Arg.Is(athlete));
        }

        [Fact]
        public async Task AddAsync_ShouldThrowException_WhenAthleteDoesNotExist()
        {
            var exception = await Record.ExceptionAsync(() => _sut.AddAsync(10, 10, 50));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(AthleteNotFoundException));
            exception.Message.ShouldBe($"Athlete with user id: '{10}' was not found.");
        }

        [Fact]
        public async Task AddAsync_ShouldThrowException_WhenAthleteExistsAndProductDoesNotExist()
        {
            var athlete = _fixture.Build<Athlete>()
                .Without(a => a.AthleteExercises)
                .Create();
            _athleteRepository.FindByCondition(Arg.Any<Expression<Func<Athlete, bool>>>(),
             Arg.Any<Func<IQueryable<Athlete>, IIncludableQueryable<Athlete, object>>>()).Returns(athlete);

            var exception = await Record.ExceptionAsync(() => _sut.AddAsync(10, 10, 50));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ProductNotFoundException));
            exception.Message.ShouldBe($"Product with id: '{10}' was not found.");
        }

        [Theory]
        [InlineData(1, 3, -150)]
        [InlineData(2, 3, -500)]
        public async Task AddAsync_ShouldThrowException_WhenAthleteExistsAndProductExistsButWeightIsNegative(int userId, int productId,
            double weight)
        {
            var athlete = _fixture.Build<Athlete>()
                .With(a => a.UserId, userId)
                .Without(a => a.AthleteExercises)
                .Create();

            var product = _fixture.Build<Product>()
                .With(p => p.Id, productId)
                .Create();

            _athleteRepository.FindByCondition(Arg.Any<Expression<Func<Athlete, bool>>>(),
                Arg.Any<Func<IQueryable<Athlete>, IIncludableQueryable<Athlete, object>>>()).Returns(athlete);

            _productRepository.GetAsync(productId).Returns(product);

            var exception = await Record.ExceptionAsync(() => _sut.AddAsync(userId, productId, weight));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(InvalidWeightException));
            exception.Message.ShouldBe("Weight cannot be less than or equal to 0.");
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        public async Task DeleteAsync_ShouldDeleteProduct_WhenAthleteExistsAndProductExists(int userId, int productId)
        {

            var athleteProducts = _fixture.Build<AthleteProduct>()
                .With(ap => ap.ProductId, productId)
                .With(ap => ap.AthleteId, userId)
                .CreateMany(count: 1);

             var athlete = _fixture.Build<Athlete>()
                .With(a => a.UserId, userId)
                .With(a => a.AthleteProducts, athleteProducts.ToList())
                .Without(a => a.AthleteExercises)
                .Create();


            _athleteRepository.FindByCondition(Arg.Any<Expression<Func<Athlete, bool>>>(),
                Arg.Any<Func<IQueryable<Athlete>, IIncludableQueryable<Athlete, object>>>()).Returns(athlete);

     
            await _sut.DeleteAsync(userId, productId);

            await _athleteRepository.UpdateAsync(Arg.Is(athlete));
        }

        [Theory]
        [InlineData(10, 5)]
        [InlineData(8, 4)]
        public async Task DeleteAsync_ShouldThrowException_WhenAthleteDoesNotExist(int userId, int productId)
        {
            var exception = await Record.ExceptionAsync(() => _sut.DeleteAsync(userId, productId));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(AthleteNotFoundException));
            exception.Message.ShouldBe($"Athlete with user id: '{userId}' was not found.");
        }

        [Theory]
        [InlineData(1, 5)]
        [InlineData(2, 15)]
        public async Task DeleteAsync_ShouldThrowException_WhenAthleteExistsAndProductDoesNotExist(int userId, int productId)
        {
             var athlete = _fixture.Build<Athlete>()
                .With(a => a.UserId, userId)
                .Without(a => a.AthleteExercises)
                .Create();

            _athleteRepository.FindByCondition(Arg.Any<Expression<Func<Athlete, bool>>>(),
                Arg.Any<Func<IQueryable<Athlete>, IIncludableQueryable<Athlete, object>>>()).Returns(athlete);

            var exception = await Record.ExceptionAsync(() => _sut.DeleteAsync(userId, productId));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ProductForAthleteNotFoundException));
            exception.Message.ShouldBe($"Product with id: '{productId}' for athlete with user id: '{userId}' was not found.");
        }
    }
}
