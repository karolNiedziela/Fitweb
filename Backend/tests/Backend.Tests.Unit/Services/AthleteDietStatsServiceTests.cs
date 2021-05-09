using AutoFixture;
using AutoMapper;
using Backend.Core.Entities;
using Backend.Core.Repositories;
using Backend.Infrastructure.Mappers;
using Backend.Infrastructure.Services;
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
    public class AthleteDietStatsServiceTests
    {
        private readonly IAthleteRepository _athleteRepository;
        private readonly IMapper _mapper;
        private readonly IFixture _fixture;
        private readonly IAthleteDietStatsService _sut;

        public AthleteDietStatsServiceTests()
        {
            _athleteRepository = Substitute.For<IAthleteRepository>();
            _mapper = AutoMapperConfig.Initialize();
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
               .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _sut = new AthleteDietStatsService(_athleteRepository, _mapper);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetDietStatsAsync_ShouldReturnAthleteDietStats(int userId)
        {
            var dietStat = _fixture.Create<DietStat>();

            var athleteDietStats = new List<AthleteDietStats>()
            {
                new AthleteDietStats
                {
                      Id = 1,
                      DateCreated = DateTime.Now,
                      DateUpdated = DateTime.Now,
                      UserId = userId,
                      DietStat = dietStat
                }
            };

            var athlete = _fixture.Build<Athlete>()
                .With(a => a.UserId, userId)
                .With(a => a.AthleteDietStats, athleteDietStats)
                .Without(a => a.AthleteExercises)
                .Create();

            _athleteRepository.FindByCondition(Arg.Any<Expression<Func<Athlete, bool>>>(),
                Arg.Any<Func<IQueryable<Athlete>, IIncludableQueryable<Athlete, object>>>()).ReturnsForAnyArgs(athlete);


            var dto = await _sut.GetDietStatsAsync(athlete.UserId, new DateTime());

            dto.ShouldNotBeNull();
            dto.UserId.ShouldBe(athlete.UserId);
            dto.DietStats.Count.ShouldBe(1);
            dto.DietStats.First().DietStat.TotalCalories = dietStat.TotalCalories;
            dto.DietStats.First().DietStat.TotalProteins = dietStat.TotalProteins;
            dto.DietStats.First().DietStat.TotalCarbohydrates = dietStat.TotalCarbohydrates;
            dto.DietStats.First().DietStat.TotalFats = dietStat.TotalFats;
        }
    }
}
