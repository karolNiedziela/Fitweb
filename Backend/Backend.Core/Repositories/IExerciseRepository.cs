using Backend.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Repositories
{
    public interface IExerciseRepository : IRepository
    {
        Task<Exercise> GetAsync(int id);
        Task<Exercise> GetAsync(string name);
        Task<IEnumerable<Exercise>> GetAllAsync();
        Task AddAsync(Exercise exercise);
        Task DeleteAsync(Exercise exercise);
        Task UpdateAsync(Exercise exercise);
    }
}
