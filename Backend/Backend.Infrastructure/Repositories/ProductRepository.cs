using Backend.Core.Entities;
using Backend.Core.Repositories;
using Backend.Infrastructure.EF;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly FitwebContext _context;

        public ProductRepository(FitwebContext context)
        {
            _context = context;
        }

        public async Task<Product> GetAsync(int id)
            => await _context.Products.Include(p => p.CategoryOfProduct).SingleOrDefaultAsync(x => x.Id == id);

        public async Task<Product> GetAsync(string name)
            => await _context.Products.Include(p => p.CategoryOfProduct).SingleOrDefaultAsync(x => x.Name == name);

        public async Task<IEnumerable<Product>> GetAllAsync()
            => await _context.Products.Include(p => p.CategoryOfProduct).ToListAsync();

        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(List<Product> products)
        {
            /*await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();*/
            await _context.BulkInsertAsync(products);
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
