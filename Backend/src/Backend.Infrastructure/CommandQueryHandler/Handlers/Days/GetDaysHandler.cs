using Backend.Core.Entities;
using Backend.Infrastructure.CommandQueryHandler.Queries.Days;
using Backend.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers.Days
{
    public class GetDaysHandler : IQueryHandler<GetDays, IEnumerable<Day>>
    {
        private readonly FitwebContext _context;

        public GetDaysHandler(FitwebContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Day>> HandleAsync(GetDays query)
        {
            return await _context.Days.ToListAsync();
        }
    }
}
