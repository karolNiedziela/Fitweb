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
    public class GetAthleteDietStatsHandler : IQueryHandler<GetDietStats, AthleteDto>
    {
        private readonly IAthleteDietStatsService _athleteDietStatsService;

        public GetAthleteDietStatsHandler(IAthleteDietStatsService athleteDietStatsService)
        {
            _athleteDietStatsService = athleteDietStatsService;
        }

        public async Task<AthleteDto> HandleAsync(GetDietStats query)
        {
            return await _athleteDietStatsService.GetDietStatsAsync(query.UserId, query.Date);
        }
    }
}
