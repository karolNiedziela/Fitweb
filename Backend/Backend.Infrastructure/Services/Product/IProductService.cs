using Backend.Core.Entities;
using Backend.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public interface IProductService
    {
        Task<ProductDetailsDto> GetAsync(int id);

        Task<ProductDetailsDto> GetAsync(string name);

        Task<IEnumerable<ProductDetailsDto>> GetAllAsync();

        Task<int> AddAsync(string name, double calories, double proteins,
            double carbohydrates, double fats, string categoryName);

        Task DeleteAsync(int id);

        Task UpdateAsync(int id, string name, double calories, double proteins, 
            double carbohydrates, double fats, string categoryName);

    }
}
