
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public interface IProductService
    {
        Task<ProductDetailsDto> GetAsync(int id);

        Task<ProductDetailsDto> GetAsync(string name);

        Task<PagedList<ProductDetailsDto>> GetAllAsync(PaginationQuery paginationQuery);

        Task<PagedList<ProductDetailsDto>> SearchAsync(PaginationQuery paginationQuery, string name, string category = null);

        Task<int> AddAsync(string name, double calories, double proteins,
            double carbohydrates, double fats, string categoryName);

        Task DeleteAsync(int id);

        Task UpdateAsync(int id, string name, double calories, double proteins, 
            double carbohydrates, double fats, string categoryName);

    }
}
