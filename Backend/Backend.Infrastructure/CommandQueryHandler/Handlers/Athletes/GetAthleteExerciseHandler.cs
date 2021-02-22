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
    public class GetAthleteExerciseHandler : IQueryHandler<GetAthleteExercise, AthleteDto>
    {
        private readonly IAthleteService _athleteService;

        public GetAthleteExerciseHandler(IAthleteService athleteService)
        {
            _athleteService = athleteService;
        }

        public async Task<AthleteDto> HandleAsync(GetAthleteExercise query)
        {
            return await _athleteService.GetExerciseAsync(query.AthleteId, query.ExerciseId);
        }
    }
}
