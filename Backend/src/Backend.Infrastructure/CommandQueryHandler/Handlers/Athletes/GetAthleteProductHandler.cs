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
    public class GetAthleteProductHandler : IQueryHandler<GetAthleteProduct, AthleteDto>
    {
        private readonly IAthleteService _athleteService;

        public GetAthleteProductHandler(IAthleteService athleteService)
        {
            _athleteService = athleteService;
        }

        public async Task<AthleteDto> HandleAsync(GetAthleteProduct query)
        {
            return await _athleteService.GetProductAsync(query.UserId, query.ProductId);
        }
    }
}
