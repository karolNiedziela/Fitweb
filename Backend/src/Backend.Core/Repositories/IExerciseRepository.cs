using Backend.Core.Entities;
using Backend.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Repositories
{
    public interface IExerciseRepository
    {
        Task<Exercise> GetAsync(int id);

        Task<Exercise> GetAsync(string name);

        Task<PagedList<Exercise>> GetAllAsync(string name, string partOfBody, PaginationQuery paginationQuery);

        Task AddAsync(Exercise exercise);

        Task AddRangeAsync(List<Exercise> exercises);

        Task DeleteAsync(Exercise exercise);

        Task UpdateAsync(Exercise exercise);

        Task<bool> AnyAsync(Expression<Func<Exercise, bool>> expression);

    }
}
