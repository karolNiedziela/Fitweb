using Backend.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public interface IAthleteService
    {
        Task<AthleteDto> GetAsync(int userId);

        Task<AthleteDto> GetProductsAsync(int userId, DateTime? date);

        Task<AthleteDto> GetProductAsync(int userId, int productId);

        Task<AthleteDto> GetExercisesAsync(int userId, string dayName);

        Task<AthleteDto> GetExerciseAsync(int userId, int exerciseId);

        Task<IEnumerable<AthleteDto>> GetAllAsync();

        Task<int> CreateAsync(int userId);

        Task DeleteAsync(int userId);
    }
}
