using AutoMapper;
using Backend.Core.Domain;
using Backend.Core.Repositories;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public class DietGoalsService : IDietGoalsService
    {
        private readonly IDietGoalsRepository _dietGoalsRepository;
        private readonly IMapper _mapper;

        public DietGoalsService(IDietGoalsRepository dietGoalsRepository, IMapper mapper)
        {
            _dietGoalsRepository = dietGoalsRepository;
            _mapper = mapper;
        }

        public async Task<DietGoalsDto> GetAsync(int userId)
        {
            var userDietGoal = await _dietGoalsRepository.GetAsync(userId);

            return _mapper.Map<DietGoalsDto>(userDietGoal);
        }

        public async Task AddAsync(int userId, double totalCalories, double proteins, double carbohydrates, double fats)
        {

            var userDietGoal = await _dietGoalsRepository.GetAsync(userId);
            if (userDietGoal != null)
            {
                userDietGoal.SetTotalCalories(totalCalories);
                userDietGoal.SetProteins(proteins);
                userDietGoal.SetCarbohydrates(carbohydrates);
                userDietGoal.SetFats(fats);
                await _dietGoalsRepository.UpdateAsync(userDietGoal);
                return;
            }

            userDietGoal = new DietGoals(totalCalories, proteins, carbohydrates, fats, userId);
            await _dietGoalsRepository.AddAsync(userDietGoal);
        }

        public async Task DeleteAsync(int userId)
        {
            var userDietGoal = await _dietGoalsRepository.GetAsync(userId);
            if (userDietGoal == null)
            {
                throw new ServiceException(Exceptions.ErrorCodes.ObjectNotFound,
                    $"User diet goal for user with id: '{userId}' was not found.");
            }

            await _dietGoalsRepository.DeleteAsync(userId);
        }

        public async Task UpdateAsync(int userId, double totalCalories, double proteins, double carbohydrates, double fats)
        {
            var userDietGoal = await _dietGoalsRepository.GetAsync(userId);
            userDietGoal.SetTotalCalories(totalCalories);
            userDietGoal.SetProteins(proteins);
            userDietGoal.SetCarbohydrates(carbohydrates);
            userDietGoal.SetFats(fats);

            await _dietGoalsRepository.UpdateAsync(userDietGoal);
        }
    }
}
