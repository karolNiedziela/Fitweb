using Backend.Core.Domain;
using Backend.Core.Repositories;
using Backend.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public class UserCalorieCounter : IUserCalorieCounter
    {
        private IUserProductRepository _userProductRepository;

        public UserCalorieCounter(IUserProductRepository userProductRepository)
        {
            _userProductRepository = userProductRepository;
        }

        public async Task<Object> CountCalories(int userId)
        {
            var userProducts = await _userProductRepository.GetAllProductsForUserAsync(userId);
            if (userProducts == null)
            {
                throw new ServiceException(ErrorCodes.ObjectNotFound, $"User with {userId} does not exist.");
            }

            var calories = 0.0;
            var proteins = 0.0;
            var carbohydrates = 0.0;
            var fats = 0.0;

            foreach (var userProduct in userProducts)
            {
                calories += userProduct.Calories;
                proteins += userProduct.Proteins;
                carbohydrates += userProduct.Carbohydrates;
                fats += userProduct.Fats;
            }

            return new { Calories = System.Math.Round(calories, 2), Proteins = System.Math.Round(proteins, 2), Carbohydrates = System.Math.Round(carbohydrates, 2), Fats = System.Math.Round(fats, 2) };
        }
    }
}
