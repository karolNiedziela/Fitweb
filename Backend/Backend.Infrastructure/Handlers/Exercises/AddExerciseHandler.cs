using Backend.Infrastructure.Commands;
using Backend.Infrastructure.Commands.Exercises;
using Backend.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Handlers.Exercises
{
    public class AddExerciseHandler : ICommandHandler<AddExercise>
    {
        private readonly IExerciseService _exerciseService;

        public AddExerciseHandler(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        public async Task HandleAsync(AddExercise command)
        {
            await _exerciseService.AddAsync(command.PartOfBody, command.Name);
        }
    }
}
