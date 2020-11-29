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
    public class ProductRepository : IProductRepository, ISqlRepository
    {
        private readonly FitwebContext _context;

        public ProductRepository(FitwebContext context)
        {
            _context = context;
        }

        public async Task<Product> GetAsync(int id)
            => await _context.Products.SingleOrDefaultAsync(x => x.Id == id);

        public async Task<Product> GetAsync(string name)
            => await _context.Products.SingleOrDefaultAsync(x => x.Name == name);
        public async Task<IEnumerable<Product>> GetAllAsync()
            => await _context.Products.ToListAsync();

        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
