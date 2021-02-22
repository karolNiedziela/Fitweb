using Backend.Infrastructure.CommandQueryHandler.Queries.Athletes;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers.Athletes
{
    public class GetAthletesHandler : IQueryHandler<GetAthletes, IEnumerable<AthleteDto>>
    {
        private readonly IAthleteService _athleteService;

        public GetAthletesHandler(IAthleteService athleteService)
        {
            _athleteService = athleteService;
        }

        public async Task<IEnumerable<AthleteDto>> HandleAsync(GetAthletes query)
        {
            return await _athleteService.GetAllAsync();
        }
    }
}
