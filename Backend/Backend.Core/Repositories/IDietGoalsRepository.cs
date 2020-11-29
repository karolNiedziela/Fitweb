using Backend.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Repositories
{
    public interface IDietGoalsRepository : IRepository
    {
        Task<DietGoals> GetAsync(int userId);
        Task AddAsync(DietGoals dietGoals);
        Task DeleteAsync(int userId);
        Task UpdateAsync(DietGoals dietGoals);
    }
}
