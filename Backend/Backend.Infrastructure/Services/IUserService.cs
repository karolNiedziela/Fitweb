using Backend.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public interface IUserService : IService
    {
        Task<UserDetailsDto> GetAsync(int id);
        Task<UserDetailsDto> GetAsync(string username);
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task RegisterAsync(string username, string email, string password, string roleName);
        Task LoginAsync(string username, string password);
        Task DeleteAsync(int userId);
        Task DeleteAsync(string username);
    }
}
