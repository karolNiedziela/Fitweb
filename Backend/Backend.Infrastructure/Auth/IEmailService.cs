using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Auth
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);

        Task Execute(string apiKey, string subject, string message, string email);

        Task<IdentityResult> ConfirmEmailAsync(int userId, string code);
    }
}
