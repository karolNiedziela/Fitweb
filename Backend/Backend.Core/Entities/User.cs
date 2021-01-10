using Backend.Core.Exceptions;
using System;
using System.Collections.Generic;

namespace Backend.Core.Entities
{
    public class User : BaseEntity
    {

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public  ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        
        public User()
        {

        }

        public User(string username, string email, string password)
        {
            SetUsername(username);
            SetEmail(email);
            SetPassword(password);
        }

        public void SetUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new DomainException(ErrorCodes.InvalidUsername, "Username cannot be empty.");
            }

            Username = username.ToLowerInvariant();
            DateUpdated = DateTime.Now;
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new DomainException(ErrorCodes.InvalidEmail, "Email cannot be empty.");
            }

            Email = email.ToLowerInvariant();
            DateUpdated = DateTime.Now;
        }

        public void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new DomainException(ErrorCodes.InvalidPassword, "Password cannot be empty.");
            }

            if (password.Length < 4)
            {
                throw new DomainException(ErrorCodes.InvalidPassword,
                    "Password cannot contain less than 4 characters.");
            }

            if (password.Length > 100)
            {
                throw new DomainException(ErrorCodes.InvalidPassword,
                    "Password cannot contain more than 100 characters.");
            }

            if (Password == password)
                return;

            Password = password;
            DateUpdated = DateTime.Now;
        }
    }
}
