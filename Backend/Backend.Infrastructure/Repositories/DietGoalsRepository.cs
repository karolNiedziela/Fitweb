using Backend.Core.Domain;
using Backend.Core.Repositories;
using Backend.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Repositories
{
    public class DietGoalsRepository : IDietGoalsRepository, ISqlRepository
    {
        private readonly FitwebContext _context;

        public DietGoalsRepository(FitwebContext context)
        {
            _context = context;
        }

        public async Task<DietGoals> GetAsync(int userId)
            => await _context.DietGoals.SingleOrDefaultAsync(x => x.UserId == userId);

        public async Task AddAsync(DietGoals dietGoals)
        {
            _context.DietGoals.Add(dietGoals);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int userId)
        {
            var userDietGoals = await GetAsync(userId);
            _context.DietGoals.Remove(userDietGoals);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DietGoals dietGoals)
        {
            _context.DietGoals.Update(dietGoals);
            await _context.SaveChangesAsync();
        }
    }
}
