using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers.Athletes
{
    public class UpdateAthleteProductHandler : ICommandHandler<UpdateAthleteProduct>
    {
        private readonly IAthleteProductService _athleteProductService;

        public UpdateAthleteProductHandler(IAthleteProductService athleteProductService)
        {
            _athleteProductService = athleteProductService;
        }

        public async Task HandleAsync(UpdateAthleteProduct command)
        {
            await _athleteProductService.UpdateAsync(command.UserId, command.ProductId, command.AthleteProductId, command.Weight);
        }
    }
}
