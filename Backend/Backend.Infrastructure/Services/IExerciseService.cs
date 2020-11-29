using Backend.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public interface IExerciseService : IService
    {
        Task<ExerciseDto> GetAsync(int id);
        Task<ExerciseDto> GetAsync(string name);
        Task<IEnumerable<ExerciseDto>> GetAllAsync();
        Task AddAsync(string partOfBody, string name);
        Task DeleteAsync(int id);
        Task DeleteAsync(string name);
        Task UpdateAsync(string partOfBody, string name);
    }
}
