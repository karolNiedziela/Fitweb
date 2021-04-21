using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers
{
    public class DeleteAthleteProductHandler : ICommandHandler<DeleteAthleteProduct>
    {
        private readonly IAthleteProductService _athleteProductService;

        public DeleteAthleteProductHandler(IAthleteProductService athleteProductService)
        {
            _athleteProductService = athleteProductService;
        }

        public async Task HandleAsync(DeleteAthleteProduct command)
        {
            await _athleteProductService.DeleteAsync(command.UserId, command.ProductId, command.AthleteProductId); 
        }
    }
}
