using Backend.Core.Entities;
using Backend.Core.Repositories;
using Backend.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;

namespace Backend.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FitwebContext _context;

        public UserRepository(FitwebContext context)
        {
            _context = context;
        }

        public async Task<User> GetAsync(int id)
            => await _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).SingleOrDefaultAsync(x => x.Id == id);

        public async Task<User> GetAsync(string value)
            => await _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .SingleOrDefaultAsync(u => u.Username == value || u.Email == value);

        public async Task<IEnumerable<User>> GetAllAsync()
            => await _context.Users.Include(x => x.UserRoles)
                                        .ThenInclude(ur => ur.Role)
                                   .AsNoTracking()
                                   .ToListAsync();

        public async Task<bool> AnyAsync(Expression<Func<User, bool>> expression)
            => await _context.Users.AnyAsync(expression);


        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }    
    }
}
