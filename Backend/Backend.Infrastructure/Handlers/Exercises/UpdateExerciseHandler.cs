using Backend.Infrastructure.Commands;
using Backend.Infrastructure.Commands.Exercises;
using Backend.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Handlers.Exercises
{
    public class UpdateExerciseHandler : ICommandHandler<UpdateExercise>
    {
        private readonly IExerciseService _exerciseService;

        public UpdateExerciseHandler(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        public async Task HandleAsync(UpdateExercise command)
        {
            await _exerciseService.UpdateAsync(command.PartOfBody, command.Name);
        }
    }
}
