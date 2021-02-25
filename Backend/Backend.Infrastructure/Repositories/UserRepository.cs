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
using Backend.Infrastructure.Exceptions;
using System.Data.SqlClient;

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
            => await _context.Users.AsNoTracking().Include(u => u.UserRoles).ThenInclude(ur => ur.Role).SingleOrDefaultAsync(x => x.Id == id);

        public async Task<User> GetByUsernameAsync(string username)
            => await _context.Users.AsNoTracking().Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .SingleOrDefaultAsync(u => u.Username == username);

        public async Task<User> GetByEmailAsync(string email)
            => await _context.Users.AsNoTracking().Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .SingleOrDefaultAsync(u => u.Email == email);

        public async Task<IEnumerable<User>> GetAllAsync()
            => await _context.Users.AsNoTracking().Include(x => x.UserRoles)
                                        .ThenInclude(ur => ur.Role)
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
