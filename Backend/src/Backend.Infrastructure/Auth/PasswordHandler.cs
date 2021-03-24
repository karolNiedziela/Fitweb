using Backend.Core.Exceptions;
using Backend.Core.Services;
using Microsoft.AspNetCore.Identity;

namespace Backend.Infrastructure.Auth
{
    public class PasswordHandler : IPasswordHandler
    {
        private readonly IPasswordHasher<IPasswordHandler> _passwordHasher;

        public PasswordHandler(IPasswordHasher<IPasswordHandler> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public bool IsValid(string hash, string password)
            => _passwordHasher.VerifyHashedPassword(this, hash, password) != PasswordVerificationResult.Failed;

    }
}
