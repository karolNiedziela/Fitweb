using Backend.Core.Entities;
using Backend.Infrastructure.DTO;
using Microsoft.AspNetCore.Identity;
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

        Task<int> SignUpAsync(string username, string email, string password, string role = "User");

        Task<IdentityResult> ChangePasswordAsync(int userId, string oldPassword, string newPassword);

        Task<IdentityResult> ConfirmEmailAsync(int userId, string code);

        Task SendConfirmationEmailAsync(User user, string token);

        Task GenerateEmailConfirmationTokenAsync(User user);
            
        Task SendForgotPasswordEmailAsync(User user, string token);

        Task GenerateForgotPasswordTokenAsync(string email);

        Task<IdentityResult> ResetPasswordAsync(int userId, string code, string newPassword);
    }
}
