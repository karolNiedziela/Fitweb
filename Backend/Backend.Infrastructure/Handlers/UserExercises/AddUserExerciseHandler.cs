using Backend.Infrastructure.Commands;
using Backend.Infrastructure.Commands.UserExercises;
using Backend.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Handlers.UserExercises
{
    public class AddUserExerciseHandler : ICommandHandler<AddUserExercise>
    {
        private readonly IUserExerciseService _userExerciseService;

        public AddUserExerciseHandler(IUserExerciseService userExerciseService)
        {
            _userExerciseService = userExerciseService;
        }

        public async Task HandleAsync(AddUserExercise command)
        {
            await _userExerciseService.AddAsync(command.UserId, command.ExerciseId, command.Weight, command.NumberOfSets, command.NumberOfReps, command.Day);
        }
    }
}
