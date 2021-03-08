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
    public class GetAthleteExercisesHandler : IQueryHandler<GetAthleteExercises, AthleteDto>
    {
        private readonly IAthleteService _athleteService;

        public GetAthleteExercisesHandler(IAthleteService athleteService)
        {
            _athleteService = athleteService;
        }

        public async Task<AthleteDto> HandleAsync(GetAthleteExercises query)
        {
            return await _athleteService.GetExercisesAsync(query.AthleteId, query.DayName);
        }
    }
}
