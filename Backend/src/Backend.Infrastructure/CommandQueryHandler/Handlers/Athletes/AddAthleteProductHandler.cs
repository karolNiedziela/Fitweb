using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers
{
    public class AddAthleteProductHandler : ICommandHandler<AddAthleteProduct>
    {
        private readonly IAthleteProductService _athleteProductService;

        public AddAthleteProductHandler(IAthleteProductService athleteProductService)
        {
            _athleteProductService = athleteProductService;
        }

        public async Task HandleAsync(AddAthleteProduct command)
        {
            await _athleteProductService.AddAsync(command.AthleteId, command.ProductId, command.Weight);
        }
    }
}
