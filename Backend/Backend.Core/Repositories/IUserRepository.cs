using Backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetAsync(int id);

        Task<User> GetAsync(string value); 

        Task<IEnumerable<User>> GetAllAsync();

        Task AddAsync(User user);

        Task RemoveAsync(User user);

        Task UpdateAsync(User user);

        Task<bool> CheckUsernameIfUsed(string username);

        Task<bool> CheckEmailIfUsed(string email);
    }
}
