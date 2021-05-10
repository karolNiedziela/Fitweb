using Backend.Core.Entities;
using Backend.Core.Repositories;
using Backend.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Repositories
{
    public class AthleteRepository : IAthleteRepository
    {
        private readonly FitwebContext _context;

        public AthleteRepository(FitwebContext context)
        {
            _context = context;
        }

        public async Task<Athlete> GetAsync(int userId)
            => await _context.Athletes.AsNoTracking().Include(a => a.CaloricDemand)
                .SingleOrDefaultAsync(a => a.UserId == userId);

        public async Task<IEnumerable<Athlete>> GetAllAsync()
         => await _context.Athletes.AsNoTracking().ToListAsync();

        // IIncludable enables to include any class, which is needed in query

        public async Task<Athlete> FindByCondition(Expression<Func<Athlete, bool>> condition,
            Func<IQueryable<Athlete>, IIncludableQueryable<Athlete, object>> include = null)
        {
            IQueryable<Athlete> query = _context.Athletes.AsNoTracking().Where(condition);

            if (include != null)
            {
                query = include(query);
            }


            return await query.FirstOrDefaultAsync();
               
        }

        public async Task AddAsync(Athlete athlete)
        {
            await _context.Athletes.AddAsync(athlete);
            await _context.SaveChangesAsync();      
        }

        public async Task DeleteAsync(Athlete athlete)
        {
            _context.Athletes.Remove(athlete);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Athlete athlete)
        {
            _context.Athletes.Update(athlete);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveProductAsync(Athlete athlete, AthleteProduct product)
        {
            _context.Athletes.Attach(athlete);

            athlete.AthleteProducts.Remove(product);

            await _context.SaveChangesAsync();
        }

        public async Task RemoveExerciseAsync(Athlete athlete, AthleteExercise exercise)
        {
            _context.Athletes.Attach(athlete);

            athlete.AthleteExercises.Remove(exercise);

            await _context.SaveChangesAsync();
        }
    }
}
