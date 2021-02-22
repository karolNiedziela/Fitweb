using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers
{
    public class AddAthleteExerciseHandler : ICommandHandler<AddAthleteExercise>
    {
        private readonly IAthleteExerciseService _athleteExerciseService;

        public AddAthleteExerciseHandler(IAthleteExerciseService athleteExerciseService)
        {
            _athleteExerciseService = athleteExerciseService;
        }

        public async Task HandleAsync(AddAthleteExercise command)
        {
            await _athleteExerciseService.AddAsync(command.AthleteId, command.ExerciseId, command.Weight,
                command.NumberOfSets, command.NumberOfReps, command.DayName);
        }
    }
}
