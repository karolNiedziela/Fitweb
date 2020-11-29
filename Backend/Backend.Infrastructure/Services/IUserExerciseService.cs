using Backend.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public interface IUserExerciseService : IService
    {
        Task<UserExercise> GetAsync(int userExerciseId);
        Task<UserExercise> GetAsync(int userId, int exerciseId);
        Task<IEnumerable<UserExercise>> GetAllAsync();
        Task<IEnumerable<UserExercise>> GetAllExercisesForUserAsync(int userId);
        Task AddAsync(int userId, int exerciseId, double weight, int numberOfSets, int numberOfReps, string dayName);
        Task DeleteAsync(int userExerciseId);
        Task UpdateAsync(int userExerciseId, int userId, int exerciseId, double weight, int numberOfSets, int numberOfReps, string dayName);
    }
}
