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

        public string Hash(string password)
        {
            ValidatePassword(password);

            return _passwordHasher.HashPassword(this, password);
        }

        public bool IsValid(string hash, string password)
            => _passwordHasher.VerifyHashedPassword(this, hash, password) != PasswordVerificationResult.Failed;

        private void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new EmptyPasswordException();
            }

            if (password.Length < 4)
            {
                throw new InvalidPasswordException("Password cannot contain less than 4 characters.");
            }

            if (password.Length > 40)
            {
                throw new InvalidPasswordException("Password cannot contain more than 40 characters.");
            }
        }
    }
}
