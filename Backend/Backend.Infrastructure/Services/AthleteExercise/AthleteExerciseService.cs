using Backend.Core.Entities;
using Backend.Core.Repositories;
using Backend.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public class AthleteExerciseService : IAthleteExerciseService
    {
        private readonly IAthleteRepository _athleteRepository;
        private readonly IExerciseRepository _exerciseRepository;

        public AthleteExerciseService(IAthleteRepository athleteRepository, IExerciseRepository exerciseRepository)
        {
            _athleteRepository = athleteRepository;
            _exerciseRepository = exerciseRepository;
        }

        public async Task AddAsync(int userId, int exerciseId, double weight, int numberOfSets, int numberOfReps, string dayName)
        {
            var athlete = await _athleteRepository.GetExercisesAsync(userId);
            if (athlete is null)
            {
                throw new ServiceException(ErrorCodes.ObjectNotFound, $"Athlete with user id: {userId} was not found.");
            }

            var exercise = await _exerciseRepository.GetAsync(exerciseId);
            if (exercise is null)
            {
                throw new ServiceException(ErrorCodes.ObjectNotFound, $"Exercise with id: {exerciseId} was not found.");
            }

            if (athlete.AthleteExercises.Any(ae => ae.ExerciseId == exerciseId && ae.DateUpdated.ToShortDateString() == DateTime.Today.ToShortDateString()))
            {
                throw new ServiceException(ErrorCodes.ObjectAlreadyAdded, $"Exercise with id: {exerciseId} already added today.");
            }


            athlete.AthleteExercises.Add(AthleteExercise.Create(athlete, exercise, weight, numberOfSets, numberOfReps, GetDay(dayName)));

            await _athleteRepository.UpdateAsync(athlete);
        }

        public async Task DeleteAsync(int userId, int exerciseId)
        {
            var athlete = await _athleteRepository.GetExercisesAsync(userId);
            if (athlete is null)
            {
                throw new ServiceException(ErrorCodes.ObjectNotFound, $"Athlete with userId: {userId} was not found.");
            }

            var exercise = athlete.AthleteExercises.SingleOrDefault(ae => ae.ExerciseId == exerciseId);
            if (exercise is null)
            {
                throw new ServiceException(ErrorCodes.ObjectNotFound, $"Exercise with id {exerciseId} for athlete with userId {userId} was not found");
            }

            athlete.AthleteExercises.Remove(exercise);

            await _athleteRepository.UpdateAsync(athlete);
        }

        private static Day GetDay(string dayName)
        {
            var days = Enum.GetValues(typeof(DayId))
                           .Cast<DayId>()
                           .Select(d => new Day()
                           {
                               Id = (int)d,
                               Name = d
                           });

            return days.SingleOrDefault(r => r.Name.ToString() == dayName);
        }
    }
}
