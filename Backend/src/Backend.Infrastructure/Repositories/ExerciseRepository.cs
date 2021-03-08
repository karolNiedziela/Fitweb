using Backend.Core.Entities;
using Backend.Core.Repositories;
using Backend.Infrastructure.EF;
using Backend.Core.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Backend.Infrastructure.Repositories
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly FitwebContext _context;

        public ExerciseRepository(FitwebContext context)
        {
            _context = context;
        }

        public async Task<Exercise> GetAsync(int id)
            => await _context.Exercises.Include(e => e.PartOfBody).AsNoTracking().SingleOrDefaultAsync(e => e.Id == id);

        public async Task<Exercise> GetAsync(string name)
            => await _context.Exercises.Include(e => e.PartOfBody).AsNoTracking().SingleOrDefaultAsync(e => e.Name == name);

        public async Task<PagedList<Exercise>> GetAllAsync(PaginationQuery paginationQuery)
            => await PagedList<Exercise>.ToPagedList(_context.Exercises
                    .Include(e => e.PartOfBody)
                    .AsNoTracking()
                    .OrderBy(e => e.Name),
                    paginationQuery.PageNumber,
                    paginationQuery.PageSize);

        public async Task<PagedList<Exercise>> SearchAsync(PaginationQuery paginationQuery, string name, string partOfBody = null)
        {
            IQueryable<Exercise> query = _context.Exercises.Include(e => e.PartOfBody)
                                            .AsNoTracking()
                                            .Where(e => e.Name.Contains(name));

            if (partOfBody is not null)
            {
                query = query.Where(e => e.PartOfBody.Name == PartOfBody.GetPart(partOfBody).Name);
            }
            
            return await PagedList<Exercise>.ToPagedList(query.OrderBy(e => e.Name),
                   paginationQuery.PageNumber,
                   paginationQuery.PageSize);
        }

        public async Task AddAsync(Exercise exercise)
        {
            await _context.Exercises.AddAsync(exercise);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(List<Exercise> exercises)
        {
            await _context.Exercises.AddRangeAsync(exercises);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Exercise exercise)
        {
            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Exercise exercise)
        {
            _context.Exercises.Update(exercise);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<Exercise, bool>> expression)
            => await _context.Exercises.AnyAsync(expression);
    }
}
