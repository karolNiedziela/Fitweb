using Backend.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public interface IProductService : IService
    {
        Task<ProductDto> GetAsync(int id);
        Task<ProductDto> GetAsync(string name);
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task AddAsync(string name, double calories, double proteins,
            double carbohydrates, double fats);
        Task DeleteAsync(int id);
        Task DeleteAsync(string name);
        Task UpdateAsync(string name, double calories, double proteins, double carbohydrates, double fats);

    }
}
