using Backend.Infrastructure.Commands;
using Backend.Infrastructure.Commands.UserExercises;
using Backend.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Handlers.UserExercises
{
    public class UpdateUserExerciseHandler : ICommandHandler<UpdateUserExercise>
    {
        private readonly IUserExerciseService _userExerciseService;

        public UpdateUserExerciseHandler(IUserExerciseService userExerciseService)
        {
            _userExerciseService = userExerciseService;
        }

        public async Task HandleAsync(UpdateUserExercise command)
        {
            await _userExerciseService.UpdateAsync(command.Id, command.UserId, command.ExerciseId, command.Weight, command.NumberOfSets, command.NumberOfReps, command.Day);
        }
    }
}
