using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public interface IAthleteProductService
    {
        Task AddAsync(int userId, int productId, double weight);

        Task DeleteAsync(int userId, int productId, int athleteProductId);
    }
}
