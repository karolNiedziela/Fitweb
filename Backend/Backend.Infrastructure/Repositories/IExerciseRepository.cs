using Backend.Core.Entities;
using Backend.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Repositories
{
    public interface IExerciseRepository
    {
        Task<Exercise> GetAsync(int id);

        Task<Exercise> GetAsync(string name);

        Task<PagedList<Exercise>> GetAllAsync(PaginationQuery paginationQuery);

        Task<PagedList<Exercise>> SearchAsync(PaginationQuery paginationQuery, string name, string partOfBody = null);

        Task AddAsync(Exercise exercise);

        Task AddRangeAsync(List<Exercise> exercises);

        Task DeleteAsync(Exercise exercise);

        Task UpdateAsync(Exercise exercise);
    }
}
