using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers
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
            command.Id = await _exerciseService.AddAsync(command.Name, command.PartOfBody);
            
        }
    }
}
