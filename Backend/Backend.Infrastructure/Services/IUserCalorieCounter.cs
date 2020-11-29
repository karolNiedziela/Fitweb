using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public interface IUserCalorieCounter : IService
    {
        Task<Object> CountCalories(int userId);
    }
}
