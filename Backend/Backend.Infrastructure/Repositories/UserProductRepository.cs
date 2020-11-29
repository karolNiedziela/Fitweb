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
    public class UserProductRepository : IUserProductRepository
    {
        private readonly FitwebContext _context;

        public UserProductRepository(FitwebContext context)
        {
            _context = context;
        }

        public async Task<UserProduct> GetAsync(int userProductId)
            => await _context.UserProducts.SingleOrDefaultAsync(x => x.Id == userProductId);

        public async Task<UserProduct> GetAsync(int userId, int productId)
            => await _context.UserProducts.SingleOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId);

        public async Task<IEnumerable<UserProduct>> GetAllAsync()
            => await _context.UserProducts.Include(x => x.Product).ToListAsync();

        public async Task<IEnumerable<UserProduct>> GetAllProductsForUserAsync(int userId)
            => await _context.UserProducts.Where(x => x.UserId == userId && x.AddedAt == DateTime.Today).ToListAsync();

        public async Task<IEnumerable<UserProduct>> GetAllProductsFromDayAsync(int userId, DateTime date)
            => await _context.UserProducts.Include(x => x.Product).Where(x => x.UserId == userId && x.AddedAt == date).ToListAsync();

        public async Task AddAsync(UserProduct userProduct)
        {
            _context.UserProducts.Add(userProduct);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(UserProduct userProduct)
        {
            _context.UserProducts.Remove(userProduct);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserProduct userProduct)
        {
            _context.UserProducts.Update(userProduct);
            await _context.SaveChangesAsync();
        }

        
    }
}
