using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services.AthleteCaloricDemand
{
    public interface ICalorieDemandService
    {
        Task SetDemandAsync(int userId, double totalCalories, double proteins, double carbohydrates, double fats);

        Task UpdateDemandAsync(int userId, double totalCalories, double proteins, double carbohydrates, double fats);
    }
}
