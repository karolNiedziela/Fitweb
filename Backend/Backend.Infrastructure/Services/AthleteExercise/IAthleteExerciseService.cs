using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public interface IAthleteExerciseService
    {
        Task AddAsync(int userId, int exerciseId, double weight, int numberOfSets, int numberOfReps, string dayName);

        Task DeleteAsync(int userId, int exerciseId);
    }
}
