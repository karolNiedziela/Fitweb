using Backend.Core.Entities;
using Backend.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public interface IAthleteDietStatsService
    {
        Task<AthleteDto> GetDietStatsAsync(int userId, DateTime? date);

        Task<AthleteDietStats> AddDietStatsAsync(int userId, Product product, double weight, char sign);
    }
}
