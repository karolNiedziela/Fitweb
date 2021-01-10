using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public class PasswordHandler : IPasswordHandler
    {
        private readonly IPasswordHasher<IPasswordHandler> _passwordHasher;

        public PasswordHandler(IPasswordHasher<IPasswordHandler> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public string Hash(string password)
            => _passwordHasher.HashPassword(this, password);

        public bool IsValid(string hash, string password)
            => _passwordHasher.VerifyHashedPassword(this, hash, password) != PasswordVerificationResult.Failed;

    }
}
