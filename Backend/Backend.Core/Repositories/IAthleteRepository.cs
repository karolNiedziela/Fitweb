using Backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Repositories
{
    public interface IAthleteRepository 
    {
        Task<Athlete> GetAsync(int userId);

        Task<IEnumerable<Athlete>> GetAllAsync();

        Task<Athlete> GetProductsAsync(int userId);

        Task<Athlete> GetProductAsync(int userId, int productId);

        Task<Athlete> GetExercisesAsync(int userId);

        Task<Athlete> GetExerciseAsync(int userId, int exerciseId);

        Task AddAsync(Athlete athlete);

        Task DeleteAsync(Athlete athlete);

        Task UpdateAsync(Athlete athlete);
    }
}
