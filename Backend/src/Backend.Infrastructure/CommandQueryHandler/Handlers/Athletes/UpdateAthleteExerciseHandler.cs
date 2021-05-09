using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.CommandQueryHandler.Commands.Athletes;
using Backend.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers.Athletes
{
    public class UpdateAthleteExerciseHandler : ICommandHandler<UpdateAthleteExercise>
    {
        private readonly IAthleteExerciseService _athleteExerciseService;

        public UpdateAthleteExerciseHandler(IAthleteExerciseService athleteExerciseService)
        {
            _athleteExerciseService = athleteExerciseService;
        }

        public async Task HandleAsync(UpdateAthleteExercise command)
        {
            await _athleteExerciseService.UpdateAsync(command.UserId, command.ExerciseId, command.Weight,
                command.NumberOfSets, command.NumberOfReps, command.DayName);
        }
    }
}
