using Backend.Core.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Repositories
{
    public interface IAthleteRepository 
    {
        Task<Athlete> GetAsync(int userId);

        Task<IEnumerable<Athlete>> GetAllAsync();

        Task<Athlete> FindByCondition(Expression<Func<Athlete, bool>> condition,
            Func<IQueryable<Athlete>, IIncludableQueryable<Athlete, object>> include = null);

        Task AddAsync(Athlete athlete);

        Task DeleteAsync(Athlete athlete);

        Task UpdateAsync(Athlete athlete);

        Task RemoveProductAsync(Athlete athlete, AthleteProduct product);
    }
}
