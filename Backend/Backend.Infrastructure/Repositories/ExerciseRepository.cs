using Backend.Core.Entities;
using Backend.Core.Repositories;
using Backend.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
            => await _context.Exercises.Include(e => e.PartOfBody).SingleOrDefaultAsync(e => e.Id == id);

        public async Task<Exercise> GetAsync(string name)
            => await _context.Exercises.Include(e => e.PartOfBody).SingleOrDefaultAsync(e => e.Name == name);

        public async Task<IEnumerable<Exercise>> GetAllAsync()
            => await _context.Exercises.Include(e => e.PartOfBody).ToListAsync();

        public async Task AddAsync(Exercise exercise)
        {
            _context.Exercises.Add(exercise);
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
    }
}
