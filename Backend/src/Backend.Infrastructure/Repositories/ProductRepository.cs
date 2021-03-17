using Backend.Core.Entities;
using Backend.Infrastructure.EF;
using Backend.Core.Helpers;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Core.Repositories;
using System.Linq.Expressions;

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
            => await _context.Products.Include(p => p.CategoryOfProduct).AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);

        public async Task<Product> GetAsync(string name)
            => await _context.Products.Include(p => p.CategoryOfProduct).AsNoTracking().SingleOrDefaultAsync(x => x.Name == name);

        public async Task<PagedList<Product>> GetAllAsync(string name, string category, PaginationQuery paginationQuery)
        {
            IQueryable<Product> query = _context.Products.Include(p => p.CategoryOfProduct)
                                                         .AsNoTracking();
                                                       

            if (name is not null)
            {
                query = query.Where(p => p.Name.Contains(name));
            }

            if (category is not null)
            {
                query = query.Where(p => p.CategoryOfProduct.Name == CategoryOfProduct.GetCategory(category).Name);
            }

            return await PagedList<Product>.ToPagedList(query.OrderBy(x => x.Name),
                                   paginationQuery.PageNumber,
                                   paginationQuery.PageSize);
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(List<Product> products)
        {
            await _context.Products.AddRangeAsync(products);
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

        public async Task<bool> AnyAsync(Expression<Func<Product, bool>> expression)
            => await _context.Products.AnyAsync(expression);
    }
}
