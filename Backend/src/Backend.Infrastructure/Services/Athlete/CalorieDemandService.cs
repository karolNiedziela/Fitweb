using Backend.Core.Entities;
using Backend.Core.Repositories;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public class CalorieDemandService : ICalorieDemandService
    {
        private readonly IAthleteRepository _athleteRepository;

        public CalorieDemandService(IAthleteRepository athleteRepository)
        {
            _athleteRepository = athleteRepository;
        }

        public async Task SetDemandAsync(int userId, double totalCalories, double proteins, double carbohydrates, double fats)
        {
            var athlete = await _athleteRepository.GetOrFailAsync(userId);

            if (athlete.CaloricDemand is not null)
            {
                await UpdateDemandAsync(userId, totalCalories, proteins, carbohydrates, fats);
                return;
            }

            athlete.CaloricDemand = CaloricDemand.Create(totalCalories, proteins, carbohydrates, fats);

            await _athleteRepository.UpdateAsync(athlete);
        }

        public async Task UpdateDemandAsync(int userId, double totalCalories, double proteins, double carbohydrates, double fats)
        {
            var athlete = await _athleteRepository.GetOrFailAsync(userId);

            athlete.CaloricDemand.SetTotalCalories(totalCalories);
            athlete.CaloricDemand.SetProteins(proteins);
            athlete.CaloricDemand.SetCarbohydrates(carbohydrates);
            athlete.CaloricDemand.SetFats(fats);

            await _athleteRepository.UpdateAsync(athlete);
        }
    }
}
