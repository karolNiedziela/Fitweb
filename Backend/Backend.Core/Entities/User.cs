using Backend.Core.Exceptions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Entities
{
    public class User : IdentityUser<int>, IBaseEntity
    {
        public bool IsExternalLoginProvider { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;

        public User()
        {

        }

        public User(string username, string email)
        {
            SetUsername(username);
            SetEmail(email);
            IsExternalLoginProvider = true;
            EmailConfirmed = true;
        }

        public User(string username, string email, string password)
        {
            SetUsername(username);
            SetEmail(email);
            SetPassword(password);
            IsExternalLoginProvider = false;
            EmailConfirmed = false;
        }

        public void SetUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new DomainException(ErrorCodes.InvalidUsername, "Username cannot be empty.");
            }

            if (UserName == username)
            {
                return;
            }

            if (username.Length < 4 || username.Length > 20)
            {
                throw new DomainException(ErrorCodes.InvalidUsername, "Username must contain at least 6 characters " +
                    "and at most twenty characters.");
            }

            UserName = username.ToLowerInvariant();
            NormalizedUserName = username.ToUpperInvariant();
            DateUpdated = DateTime.UtcNow;
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new DomainException(ErrorCodes.InvalidEmail, "Email cannot be empty.");
            }

            if (Email == email)
            {
                return;
            }

            EmailAddressAttribute emailAddress = new EmailAddressAttribute();

            if (!emailAddress.IsValid(email))
            {
                throw new DomainException(ErrorCodes.InvalidEmail, "Invalid email format.");
            }

            Email = email.ToLowerInvariant();
            NormalizedEmail = email.ToUpperInvariant();
            DateUpdated = DateTime.UtcNow;
        }

        public void SetPassword(string password)
        {
            if (PasswordHash == password)
                return;

            PasswordHash = password;
            DateUpdated = DateTime.UtcNow;
        }
    }
}
