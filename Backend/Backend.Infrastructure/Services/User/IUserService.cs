using Backend.Core.Entities;
using Backend.Infrastructure.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public interface IUserService
    {
        Task<UserDto> GetAsync(int id);

        Task<UserDto> GetByUsernameAsync(string username);

        Task<IEnumerable<UserDto>> GetAllAsync();

        Task DeleteAsync(int userId);

        Task UpdateAsync(int id, string username, string email, string password);
    }
}
