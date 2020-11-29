using Backend.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Repositories
{
    public interface IUserProductRepository : IRepository
    {
        Task<UserProduct> GetAsync(int userProductId);
        Task<UserProduct> GetAsync(int userId, int productId);
        Task<IEnumerable<UserProduct>> GetAllAsync();
        Task<IEnumerable<UserProduct>> GetAllProductsForUserAsync(int userId);
        Task<IEnumerable<UserProduct>> GetAllProductsFromDayAsync(int userId, DateTime date);
        Task AddAsync(UserProduct userProduct);
        Task DeleteAsync(UserProduct userProduct);
        Task UpdateAsync(UserProduct userProduct);
    }
}
