using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public interface IAthleteExerciseService
    {
        Task AddAsync(int athleteId, int exerciseId, double weight, int numberOfSets, int numberOfReps, string dayName);

        Task DeleteAsync(int athleteId, int exerciseId);
    }
}
