using Backend.Core.Domain;
using Backend.Core.Repositories;
using Backend.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Repositories
{
    public class UserExerciseRepository : IUserExerciseRepository, ISqlRepository
    {
        private readonly FitwebContext _context;

        public UserExerciseRepository(FitwebContext context) 
        {
            _context = context;
        }

        public async Task<UserExercise> GetAsync(int userExerciseId)
            => await _context.UserExercises.Include(x => x.Day).SingleOrDefaultAsync(x => x.Id == userExerciseId);

        public async Task<UserExercise> GetAsync(int userId, int exerciseId)
            => await _context.UserExercises.Include(x => x.Day).SingleOrDefaultAsync(x => x.UserId == userId && x.ExerciseId == exerciseId);

        public async Task<IEnumerable<UserExercise>> GetAllExercisesForUserAsync(int userId)
            => await _context.UserExercises.Include(x => x.Day).Include(x => x.Exercise).Where(x => x.UserId == userId).ToListAsync();

        public async Task<IEnumerable<UserExercise>> GetAllAsync()
            => await _context.UserExercises.Include(x => x.Exercise).ToListAsync();

        public async Task AddAsync(UserExercise userExercise)
        {
            _context.UserExercises.Add(userExercise);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(UserExercise userExercise)
        {
            _context.UserExercises.Remove(userExercise);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(UserExercise userExercise)
        {
            _context.UserExercises.Update(userExercise);
            await _context.SaveChangesAsync();
        }

        
    }
}
