using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers
{
    public class DeleteAthleteExerciseHandler : ICommandHandler<DeleteAthleteExercise>
    {
        private readonly IAthleteExerciseService _athleteExerciseService;

        public DeleteAthleteExerciseHandler(IAthleteExerciseService athleteExerciseService)
        {
            _athleteExerciseService = athleteExerciseService;
        }

        public async Task HandleAsync(DeleteAthleteExercise command)
        {
            await _athleteExerciseService.DeleteAsync(command.UserId, command.ExerciseId);
        }
    }
}
