using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public interface IAthleteProductService
    {
        Task AddAsync(int athleteId, int productId, double weight);

        Task DeleteAsync(int athleteId, int productId);
    }
}
