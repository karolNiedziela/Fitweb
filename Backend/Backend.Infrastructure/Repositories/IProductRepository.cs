using Backend.Core.Entities;
using Backend.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetAsync(int id);

        Task<Product> GetAsync(string name);

        Task<PagedList<Product>> GetAllAsync(PaginationQuery paginationQuery);

        Task<PagedList<Product>> SearchAsync(PaginationQuery paginationQuery, string name, string category = null);

        Task AddAsync(Product product);

        Task AddRangeAsync(List<Product> products);

        Task DeleteAsync(Product product);

        Task UpdateAsync(Product product);
    }
}
