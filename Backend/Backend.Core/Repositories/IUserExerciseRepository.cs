using Backend.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Repositories
{
    public interface IUserExerciseRepository : IRepository
    {
        Task<UserExercise> GetAsync(int userExerciseId);
        Task<UserExercise> GetAsync(int userId, int exerciseId);
        Task<IEnumerable<UserExercise>> GetAllExercisesForUserAsync(int userId);
        Task<IEnumerable<UserExercise>> GetAllAsync();
        Task AddAsync(UserExercise userProduct);
        Task DeleteAsync(UserExercise userProduct);
        Task UpdateAsync(UserExercise userExercise);
    }
}
