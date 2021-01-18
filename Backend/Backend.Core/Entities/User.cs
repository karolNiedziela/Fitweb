using Backend.Core.Exceptions;
using System;
using System.Collections.Generic;

namespace Backend.Core.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }

        public string Email { get; set; }

#nullable enable
        public string? Password { get; set; }

        public bool IsExternalLoginProvider { get; set; }

        public  ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        
        public User()
        {

        }

        public User(string username, string email)
        {
            SetUsername(username);
            SetEmail(email);
            IsExternalLoginProvider = true;
        }

        public User(string username, string email, string password)
        {
            SetUsername(username);
            SetEmail(email);
            SetPassword(password);
            IsExternalLoginProvider = false;
        }

        public void SetUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new DomainException(ErrorCodes.InvalidUsername, "Username cannot be empty.");
            }

            Username = username;
            DateUpdated = DateTime.Now;
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new DomainException(ErrorCodes.InvalidEmail, "Email cannot be empty.");
            }

            Email = email;
            DateUpdated = DateTime.Now;
        }

        public void SetPassword(string password)
        {
            if (Password == password)
                return;

            Password = password;
            DateUpdated = DateTime.Now;
        }

    }
}
