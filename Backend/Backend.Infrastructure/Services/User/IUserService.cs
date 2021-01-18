using Backend.Core.Entities;
using Backend.Infrastructure.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public interface IUserService
    {
        Task<UserDto> GetAsync(int id);

        Task<UserDto> GetAsync(string username);

        Task<IEnumerable<UserDto>> GetAllAsync();

        Task<int> RegisterAsync(string username, string email, string password = null, string roleName = "User");

        Task LoginAsync(string username, string password);

        Task DeleteAsync(int userId);

        Task UpdateAsync(int id, string username, string email, string password);
    }
}
