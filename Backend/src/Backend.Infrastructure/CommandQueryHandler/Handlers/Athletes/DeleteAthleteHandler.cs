using Backend.Infrastructure.CommandQueryHandler.Commands.Athletes;
using Backend.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers.Athletes
{
    public class DeleteAthleteHandler : ICommandHandler<DeleteAthlete>
    {
        private readonly IAthleteService _athleteService;

        public DeleteAthleteHandler(IAthleteService athleteService)
        {
            _athleteService = athleteService;
        }

        public async Task HandleAsync(DeleteAthlete command)
        {
            await _athleteService.DeleteAsync(command.AthleteId);
        }
    }
}
