
using Backend.Infrastructure.DTO;
using Backend.Core.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public interface IProductService
    {
        Task<ProductDto> GetAsync(int id);

        Task<ProductDto> GetAsync(string name);

        Task<PagedList<ProductDto>> GetAllAsync(string name, string category, PaginationQuery paginationQuery);

        Task<int> AddAsync(string name, double calories, double proteins,
            double carbohydrates, double fats, string categoryName);

        Task DeleteAsync(int id);

        Task UpdateAsync(int id, string name, double calories, double proteins, 
            double carbohydrates, double fats, string categoryName);

    }
}
