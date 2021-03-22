using Backend.Core.Entities;
using Backend.Core.Exceptions;
using Backend.Core.Repositories;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Extensions;
using Backend.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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
            var athlete = await _athleteRepository.FindByCondition(condition: a => a.UserId == userId,
                include: source => source.Include(a => a.AthleteExercises)
                                                    .ThenInclude(ae => ae.Day)
                                                  .Include(a => a.AthleteExercises.Where(ae => ae.ExerciseId == exerciseId))
                                                    .ThenInclude(ae => ae.Exercise)
                                                        .ThenInclude(e => e.PartOfBody));

            if (athlete is null)
            {
                throw new AthleteNotFoundException(userId);
            }

            if (athlete.AthleteExercises.Any(ae => ae.ExerciseId == exerciseId && ae.DateUpdated.ToShortDateString() == DateTime.Today.ToShortDateString()))
            {
                throw new AlreadyAddedTodayException(nameof(Exercise), exerciseId);
            }

            var exercise = await _exerciseRepository.GetOrFailAsync(exerciseId);

            athlete.AthleteExercises.Add(AthleteExercise.Create(athlete, exercise, weight, numberOfSets, numberOfReps, Day.GetDay(dayName)));

            await _athleteRepository.UpdateAsync(athlete);
        }

        public async Task DeleteAsync(int userId, int exerciseId)
        {
            var athlete = await _athleteRepository.FindByCondition(condition: a => a.UserId == userId,
                include: source => source.Include(a => a.AthleteExercises)
                                                    .ThenInclude(ae => ae.Day)
                                                  .Include(a => a.AthleteExercises)
                                                    .ThenInclude(ae => ae.Exercise)
                                                        .ThenInclude(e => e.PartOfBody));
            if (athlete is null)
            {
                throw new AthleteNotFoundException(userId);
            }

            var exercise = athlete.AthleteExercises.SingleOrDefault(ae => ae.ExerciseId == exerciseId);
            if (exercise is null)
            {
                throw new ExerciseForAthleteNotFoundException(userId, exerciseId);
            }

            athlete.AthleteExercises.Remove(exercise);

            await _athleteRepository.UpdateAsync(athlete);
        }

    }
}
