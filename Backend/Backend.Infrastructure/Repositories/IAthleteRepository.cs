using Backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Repositories
{
    public interface IAthleteRepository 
    {
        Task<Athlete> GetAsync(int userId);

        Task<IEnumerable<Athlete>> GetAllAsync();

        IQueryable<Athlete> FindByCondition(Expression<Func<Athlete, bool>> expression);

        Task AddAsync(Athlete athlete);

        Task DeleteAsync(Athlete athlete);

        Task UpdateAsync(Athlete athlete);
    }
}
