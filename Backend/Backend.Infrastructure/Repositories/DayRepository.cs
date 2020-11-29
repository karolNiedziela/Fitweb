using Backend.Core.Domain;
using Backend.Core.Repositories;
using Backend.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Repositories
{
    public class DayRepository : IDayRepository, ISqlRepository
    {
        private readonly FitwebContext _context;

        public DayRepository(FitwebContext context)
        {
            _context = context;
        }


        public async Task<Day> GetAsync(string name)
            => await _context.Days.SingleOrDefaultAsync(x => x.Name == name);
        public async Task<IEnumerable<Day>> GetAllAsync()
            => await _context.Days.ToListAsync();

    }
}
