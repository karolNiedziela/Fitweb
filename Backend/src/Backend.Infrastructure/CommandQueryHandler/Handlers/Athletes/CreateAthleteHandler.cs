using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers
{
    public class CreateAthleteHandler : ICommandHandler<CreateAthlete>
    {
        private readonly IAthleteService _athleteService;

        public CreateAthleteHandler(IAthleteService athleteService)
        {
            _athleteService = athleteService;
        }

        public async Task HandleAsync(CreateAthlete command)
        {
            await _athleteService.CreateAsync(command.UserId);
        }
    }
}
