using Backend.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public interface IAthleteService
    {
        Task<AthleteDto> GetAsync(int id);

        Task<AthleteDto> GetProductsAsync(int id, DateTime? date);

        Task<AthleteDto> GetProductAsync(int id, int productId);

        Task<AthleteDto> GetExercisesAsync(int id, string dayName);

        Task<AthleteDto> GetExerciseAsync(int id, int exerciseId);

        Task<IEnumerable<AthleteDto>> GetAllAsync();

        Task<int> CreateAsync(int userId);

        Task DeleteAsync(int id);
    }
}
