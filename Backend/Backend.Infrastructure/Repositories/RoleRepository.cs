using Backend.Core.Domain;
using Backend.Core.Repositories;
using Backend.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository, ISqlRepository
    {
        private readonly FitwebContext _context;

        public RoleRepository(FitwebContext context)
        {
            _context = context;
        }

        public async Task<Role> GetAsync(string name)
            => await _context.Roles.SingleOrDefaultAsync(x => x.Name == name);
    }
}
