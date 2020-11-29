using Backend.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public interface IUserProductService : IService
    {
        Task<UserProductDto> GetAsync(int userProductId);
        Task<UserProductDto> GetAsync(int userId, int productId);
        Task<IEnumerable<UserProductDetailsDto>> GetAllAsync();
        Task<UserDetailsDto> GetAllUserProducts(int userId);
        Task<IEnumerable<UserProductDetailsDto>> GetAllUserProductsFromDay(int userId, string date);
        
        Task AddAsync(int userId, int productId, double weight);
        Task DeleteAsync(int userProductId);
        Task UpdateAsync(int userId, int productId, double weight);
    }
}
