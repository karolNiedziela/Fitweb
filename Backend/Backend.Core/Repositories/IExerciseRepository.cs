using Backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Repositories
{
    public interface IExerciseRepository
    {
        Task<Exercise> GetAsync(int id);

        Task<Exercise> GetAsync(string name);

        Task<IEnumerable<Exercise>> GetAllAsync();

        Task AddAsync(Exercise exercise);

        Task DeleteAsync(Exercise exercise);

        Task UpdateAsync(Exercise exercise);
    }
}
