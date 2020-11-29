using Backend.Infrastructure.Commands;
using Backend.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Handlers.Account
{
    public class AddDietGoalsHandler : ICommandHandler<AddDietGoals>
    {
        private readonly IDietGoalsService _dietGoalsService;

        public AddDietGoalsHandler(IDietGoalsService dietGoalsService)
        {
            _dietGoalsService = dietGoalsService;
        }


        public async Task HandleAsync(AddDietGoals command)
        {
            await _dietGoalsService.AddAsync(command.UserId, command.TotalCalories, command.Proteins, command.Carbohydrates, command.Fats);
        }
    }
}
