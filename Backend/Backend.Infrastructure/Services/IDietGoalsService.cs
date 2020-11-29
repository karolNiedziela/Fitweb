using Backend.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public interface IDietGoalsService : IService
    {
        Task<DietGoalsDto> GetAsync(int userId);
        Task AddAsync(int userId, double totalCalories, double proteins, double carbohydrates, double fats);
        Task DeleteAsync(int userId);
        Task UpdateAsync(int userId, double totalCalories, double proteins, double carbohydrates, double fats);
    }
}
