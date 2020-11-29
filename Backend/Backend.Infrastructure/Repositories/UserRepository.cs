using Backend.Core.Domain;
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
    public class UserRepository : IUserRepository, ISqlRepository
    {
        private readonly FitwebContext _context;

        public UserRepository(FitwebContext context)
        {
            _context = context;
        }

        public async Task<User> GetAsync(int id)
            => await _context.Users.Include(x => x.Role).SingleOrDefaultAsync(x => x.Id == id);

        public async Task<User> GetAsync(string value)
            => await _context.Users.Include(x => x.Role)
            .Include(x => x.Products).ThenInclude(xp => xp.Product)
            .Include(x => x.Exercises).ThenInclude(xe => xe.Exercise)
            .Include(x => x.Exercises).ThenInclude(xe => xe.Day)
            .SingleOrDefaultAsync(x => x.Username == value || x.Email == value);

        public async Task<IEnumerable<User>> GetAllAsync()
            => await _context.Users.Include(x => x.Role).ToListAsync();

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
    }
}
