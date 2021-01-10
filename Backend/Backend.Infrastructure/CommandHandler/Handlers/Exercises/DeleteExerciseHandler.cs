using Backend.Infrastructure.CommandHandler.Commands;
using Backend.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandHandler.Handlers
{
    public class DeleteExerciseHandler : ICommandHandler<DeleteExercise>
    {
        private readonly IExerciseService _exerciseService;

        public DeleteExerciseHandler(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        public async Task HandleAsync(DeleteExercise command)
        {
            await _exerciseService.DeleteAsync(command.ExerciseId);
        }
    }
}
