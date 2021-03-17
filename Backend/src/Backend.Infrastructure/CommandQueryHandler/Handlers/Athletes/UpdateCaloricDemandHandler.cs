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
    public class UpdateCaloricDemandHandler : ICommandHandler<UpdateCaloricDemand>
    {
        private readonly ICalorieDemandService _calorieDemandService;

        public UpdateCaloricDemandHandler(ICalorieDemandService calorieDemandService)
        {
            _calorieDemandService = calorieDemandService;
        }

        public async Task HandleAsync(UpdateCaloricDemand command)
        {
            await _calorieDemandService.UpdateDemandAsync(command.UserId, command.TotalCalories, command.Proteins,
                command.Carbohydrates, command.Fats);
        }
    }
}
