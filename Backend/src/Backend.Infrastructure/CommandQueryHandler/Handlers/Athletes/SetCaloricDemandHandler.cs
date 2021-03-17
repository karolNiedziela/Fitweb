using Backend.Core.Entities;
using Backend.Infrastructure.CommandQueryHandler.Commands.Athletes;
using Backend.Infrastructure.Services.AthleteCaloricDemand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers
{
    public class SetCaloricDemandHandler : ICommandHandler<SetCaloricDemand>
    {
        private readonly ICalorieDemandService _calorieDemandService;

        public SetCaloricDemandHandler(ICalorieDemandService calorieDemandService)
        {
            _calorieDemandService = calorieDemandService;
        } 

        public async Task HandleAsync(SetCaloricDemand command)
        {
            await _calorieDemandService.SetDemandAsync(command.UserId, command.TotalCalories,
                command.Proteins, command.Carbohydrates, command.Fats);
        }
    }
}
