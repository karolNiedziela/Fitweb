using AutoMapper;
using Backend.Core.Entities;
using Backend.Core.Repositories;
using Backend.Infrastructure.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public class AthleteDietStatsService : IAthleteDietStatsService
    {
        private readonly IAthleteRepository _athleteRepository;
        private readonly IMapper _mapper;

        public AthleteDietStatsService(IAthleteRepository athleteRepository, IMapper mapper)
        {
            _athleteRepository = athleteRepository;
            _mapper = mapper;
        }

        public async Task<AthleteDto> GetDietStatsAsync(int userId, DateTime? date)
        {
            if (date is null)
            {
                date = DateTime.Today;
            }

            var start = date;
            var end = date?.AddDays(1);

            var athlete = await _athleteRepository.FindByCondition(a => a.UserId == userId,
                include: source => source.Include(a => a.AthleteDietStats
                    .Where(ads => ads.DateCreated >= start && ads.DateCreated <= end)).ThenInclude(ads => ads.DietStat));


            return _mapper.Map<AthleteDto>(athlete);                      
        }

        public async Task<AthleteDietStats> AddDietStatsAsync(int userId, Product product, double weight, char sign,
            DateTime dateCreated)
        {
            var start = new DateTime(dateCreated.Year, dateCreated.Month, dateCreated.Day, 0, 0, 0);
            var end = start.AddDays(1);
            var athlete = await _athleteRepository.FindByCondition(a => a.UserId == userId,
                include: source => source.Include(a => a.AthleteDietStats
                    .Where(ads => ads.DateCreated >= start && ads.DateCreated <= end)).ThenInclude(ads => ads.DietStat));

            if (athlete.AthleteDietStats.Count is 0)
            {
                athlete.AthleteDietStats.Add(AthleteDietStats.Create(athlete, new DietStat()));
            }

            var dietStat = athlete.AthleteDietStats.Select(ads => ads.DietStat).FirstOrDefault();

            Calculate(ref dietStat, product, weight, sign);

            var result = athlete.AthleteDietStats.FirstOrDefault();

            return result;
        }

        private static void Calculate(ref DietStat dietStat, Product product, double weight, char sign)
        {
            if (sign == '+')
            {
                dietStat.TotalCalories += product.Calories * weight / 100;
                dietStat.TotalProteins += product.Proteins * weight / 100;
                dietStat.TotalCarbohydrates += product.Carbohydrates * weight / 100;
                dietStat.TotalFats += product.Fats * weight / 100;
            }
           
            if (sign == '-')
            {
                dietStat.TotalCalories -= product.Calories * weight / 100;
                dietStat.TotalProteins -= product.Proteins * weight / 100;
                dietStat.TotalCarbohydrates -= product.Carbohydrates * weight / 100;
                dietStat.TotalFats -= product.Fats * weight / 100;
            }
        }
    }
}
