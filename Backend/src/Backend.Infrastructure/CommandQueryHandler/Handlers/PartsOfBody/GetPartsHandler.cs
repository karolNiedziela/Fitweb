using Backend.Core.Entities;
using Backend.Infrastructure.CommandQueryHandler.Queries.PartsOfBody;
using Backend.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers.PartsOfBody
{
    public class GetPartsHandler : IQueryHandler<GetParts, IEnumerable<PartOfBody>>
    {
        private readonly FitwebContext _context;

        public GetPartsHandler(FitwebContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PartOfBody>> HandleAsync(GetParts query)
        {
            return await _context.PartOfBodies.OrderBy(pob => pob.Name).ToListAsync();
        }
    }
}
