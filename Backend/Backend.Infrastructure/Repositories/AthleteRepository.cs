using Backend.Core.Entities;
using Backend.Core.Repositories;
using Backend.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
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
            => await _context.Athletes.SingleOrDefaultAsync(a => a.UserId == userId);

        public async Task<IEnumerable<Athlete>> GetAllAsync()
         => await _context.Athletes.ToListAsync();

        public IQueryable<Athlete> FindByCondition(Expression<Func<Athlete, bool>> expression)
            => _context.Athletes.Where(expression).AsNoTracking();

        public async Task<Athlete> GetProductsAsync(int userId)
            => await _context.Athletes.Include(a => a.AthleteProducts)
                                        .ThenInclude(ap => ap.Product)
                                       .SingleOrDefaultAsync(a => a.UserId == userId);

        public async Task<Athlete> GetProductAsync(int userId, int productId)
          => await _context.Athletes.Where(a => a.UserId == userId)
                           .Include(a => a.AthleteProducts.Where(ap => ap.ProductId == productId))
                            .ThenInclude(ap => ap.Product)
                           .SingleOrDefaultAsync();

        public async Task<Athlete> GetExercisesAsync(int userId)
            => await _context.Athletes.Include(a => a.AthleteExercises)
                                        .ThenInclude(ae => ae.Exercise)
                                      .Include(a => a.AthleteExercises)
                                        .ThenInclude(ae => ae.Day)
                                      .SingleOrDefaultAsync(a => a.UserId == userId);

        public async Task<Athlete> GetExerciseAsync(int userId, int exerciseId)
           => await _context.Athletes.Where(a => a.UserId == userId)
                            .Include(a => a.AthleteExercises.Where(ae => ae.ExerciseId == exerciseId))
                                .ThenInclude(ae => ae.Exercise)
                            .Include(a => a.AthleteExercises)
                                .ThenInclude(ae => ae.Day)
                            .SingleOrDefaultAsync();

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
    }
}
