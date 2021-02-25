using Backend.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services.Account
{
    public interface IAccountService
    {
        Task<JwtDto> SignInAsync(string username, string password);

        Task<int> SignUpAsync(string username, string email, string password, string roleName = "User");

        Task ChangePasswordAsync(int userId, string oldPassword, string newPassword);
    }
}
