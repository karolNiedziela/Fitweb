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
    public class AthleteProductServiceTests : IClassFixture<FitwebFixture>
    {
        private readonly IAthleteRepository _athleteRepository;
        private readonly IProductRepository _productRepository;
        private readonly IAthleteProductService _sut;
        private readonly FitwebFixture _fixture;

        public AthleteProductServiceTests(FitwebFixture fixture)
        {
            _fixture = fixture;
            _athleteRepository = Substitute.For<IAthleteRepository>();
            _productRepository = Substitute.For<IProductRepository>();
            _sut = new AthleteProductService(_athleteRepository, _productRepository);
        }

        [Theory]
        [InlineData(1, 3, 150)]
        [InlineData(2, 3, 500)]
        public async Task AddAsync_ShouldAddProduct_WhenAthleteExistsAndProductExistsAndWeightIsValid(int athleteId, int productId, 
            double weight)
        {
            var athlete = _fixture.FitwebContext.Athletes.Where(a => a.Id == athleteId)
                                                .Include(a => a.AthleteProducts.Where(ap => ap.ProductId == productId))
                                                    .ThenInclude(ap => ap.Product)
                                                    .ThenInclude(p => p.CategoryOfProduct)
                                                .AsNoTracking()
                                                .SingleOrDefault();

            var product = _fixture.FitwebContext.Products.Include(p => p.CategoryOfProduct).SingleOrDefault(p => p.Id == productId);

            _athleteRepository.FindByCondition(Arg.Any<Expression<Func<Athlete, bool>>>(),
                Arg.Any<Func<IQueryable<Athlete>, IIncludableQueryable<Athlete, object>>>()).Returns(athlete);

            _productRepository.GetAsync(productId).Returns(product);

            await _sut.AddAsync(athleteId, productId, weight);

            await _athleteRepository.Received(1).UpdateAsync(Arg.Is(athlete));
        }

        [Fact]
        public async Task AddAsync_ShouldThrowException_WhenAthleteDoesNotExist()
        {
            var exception = await Record.ExceptionAsync(() => _sut.AddAsync(10, 10, 50));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            ((ServiceException)exception).Code.ShouldBe(Infrastructure.Exceptions.ErrorCodes.AthleteNotFound);
            exception.Message.ShouldBe($"Athlete with id: {10} was not found.");
        }

        [Fact]
        public async Task AddAsync_ShouldThrowException_WhenAthleteExistsAndProductDoesNotExist()
        {
            var athlete = _fixture.FitwebContext.Athletes.AsNoTracking().Where(a => a.Id == 1)
                                                .Include(a => a.AthleteProducts.Where(ap => ap.ProductId == 1))
                                                    .ThenInclude(ap => ap.Product)
                                                    .ThenInclude(p => p.CategoryOfProduct)
                                                .SingleOrDefault();

            _athleteRepository.FindByCondition(Arg.Any<Expression<Func<Athlete, bool>>>(),
             Arg.Any<Func<IQueryable<Athlete>, IIncludableQueryable<Athlete, object>>>()).Returns(athlete);

            var exception = await Record.ExceptionAsync(() => _sut.AddAsync(10, 10, 50));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            ((ServiceException)exception).Code.ShouldBe(Infrastructure.Exceptions.ErrorCodes.ObjectNotFound);
            exception.Message.ShouldBe($"Product with id: {10} was not found.");
        }

        [Theory]
        [InlineData(1, 3, -150)]
        [InlineData(2, 3, -500)]
        public async Task AddAsync_ShouldThrowException_WhenAthleteExistsAndProductExistsButWeightIsNegative(int athleteId, int productId,
            double weight)
        {
            var athlete = _fixture.FitwebContext.Athletes.AsNoTracking().Where(a => a.Id == athleteId)
                                               .Include(a => a.AthleteProducts.Where(ap => ap.ProductId == productId))
                                                   .ThenInclude(ap => ap.Product)
                                                   .ThenInclude(p => p.CategoryOfProduct)
                                               .SingleOrDefault();

            var product = _fixture.FitwebContext.Products.Include(p => p.CategoryOfProduct).SingleOrDefault(p => p.Id == productId);

            _athleteRepository.FindByCondition(Arg.Any<Expression<Func<Athlete, bool>>>(),
                Arg.Any<Func<IQueryable<Athlete>, IIncludableQueryable<Athlete, object>>>()).Returns(athlete);

            _productRepository.GetAsync(productId).Returns(product);

            var exception = await Record.ExceptionAsync(() => _sut.AddAsync(athleteId, productId, weight));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(DomainException));
            ((DomainException)exception).Code.ShouldBe(Core.Exceptions.ErrorCodes.InvalidValue);
            exception.Message.ShouldBe("Weight cannot be less than or equal to 0.");
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        public async Task DeleteAsync_ShouldDeleteProduct_WhenAthleteExistsAndProductExists(int athleteId, int productId)
        {
            var athlete = _fixture.FitwebContext.Athletes.AsNoTracking().Where(a => a.Id == athleteId)
                                               .Include(a => a.AthleteProducts.Where(ap => ap.ProductId == productId))
                                                   .ThenInclude(ap => ap.Product)
                                                   .ThenInclude(p => p.CategoryOfProduct)
                                               .SingleOrDefault();

            _athleteRepository.FindByCondition(Arg.Any<Expression<Func<Athlete, bool>>>(),
                Arg.Any<Func<IQueryable<Athlete>, IIncludableQueryable<Athlete, object>>>()).Returns(athlete);

            var product = athlete.AthleteProducts.SingleOrDefault(ap => ap.ProductId == productId);

            await _sut.DeleteAsync(athleteId, productId);

            await _athleteRepository.UpdateAsync(Arg.Any<Athlete>());
        }

        [Theory]
        [InlineData(10, 5)]
        [InlineData(8, 4)]
        public async Task DeleteAsync_ShouldThrowException_WhenAthleteDoesNotExist(int athleteId, int productId)
        {
            var exception = await Record.ExceptionAsync(() => _sut.DeleteAsync(athleteId, productId));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            ((ServiceException)exception).Code.ShouldBe(Infrastructure.Exceptions.ErrorCodes.ObjectNotFound);
            exception.Message.ShouldBe($"Athlete with id: {athleteId} was not found.");
        }

        [Theory]
        [InlineData(1, 5)]
        [InlineData(2, 15)]
        public async Task DeleteAsync_ShouldThrowException_WhenAthleteExistsAndProductDoesNotExist(int athleteId, int productId)
        {
            var athlete = _fixture.FitwebContext.Athletes.AsNoTracking().Where(a => a.Id == athleteId)
                                               .Include(a => a.AthleteProducts.Where(ap => ap.ProductId == productId))
                                                   .ThenInclude(ap => ap.Product)
                                                   .ThenInclude(p => p.CategoryOfProduct)
                                               .SingleOrDefault();

            _athleteRepository.FindByCondition(Arg.Any<Expression<Func<Athlete, bool>>>(),
                Arg.Any<Func<IQueryable<Athlete>, IIncludableQueryable<Athlete, object>>>()).Returns(athlete);

            var exception = await Record.ExceptionAsync(() => _sut.DeleteAsync(athleteId, productId));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            ((ServiceException)exception).Code.ShouldBe(Infrastructure.Exceptions.ErrorCodes.ObjectNotFound);
            exception.Message.ShouldBe($"Product with id {productId} for athlete with id {athleteId} was not found.");
        }
    }
}
