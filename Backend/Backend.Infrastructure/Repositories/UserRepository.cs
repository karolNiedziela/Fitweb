using Backend.Core.Entities;
using Backend.Core.Repositories;
using Backend.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

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
            => await _context.Users.Include(x => x.UserRoles).ThenInclude(ur => ur.Role).ToListAsync();

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckUsernameIfUsed(string username)
            => await _context.Users.AnyAsync(u => u.Username == username);

        public async Task<bool> CheckEmailIfUsed(string email)
            => await _context.Users.AnyAsync(u => u.Email == email);
    }
}
