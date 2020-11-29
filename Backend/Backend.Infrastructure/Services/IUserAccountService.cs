using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public interface IUserAccountService : IService
    {
        Task ChangePasswordAsync(int userId, string newPassword);
        Task EditProfile(int userId, string username, string email);

    }
}
