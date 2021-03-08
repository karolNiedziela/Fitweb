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
    public class GetAthleteProductsHandler : IQueryHandler<GetAthleteProducts, AthleteDto>
    {
        private readonly IAthleteService _athleteService;

        public GetAthleteProductsHandler(IAthleteService athleteService)
        {
            _athleteService = athleteService;
        }

        public async Task<AthleteDto> HandleAsync(GetAthleteProducts query)
        {
            return await _athleteService.GetProductsAsync(query.AthleteId, query.Date);
        }
    }
}
