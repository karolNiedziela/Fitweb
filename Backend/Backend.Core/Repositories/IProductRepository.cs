using Backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetAsync(int id);

        Task<Product> GetAsync(string name);

        Task<IEnumerable<Product>> GetAllAsync();

        Task AddAsync(Product product);

        Task AddRangeAsync(List<Product> products);

        Task DeleteAsync(Product product);

        Task UpdateAsync(Product product);
    }
}
