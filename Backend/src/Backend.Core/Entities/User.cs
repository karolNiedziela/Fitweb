using Backend.Core.Exceptions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Entities
{
    public class User : IdentityUser<int>
    {

        public bool IsExternalLoginProvider { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public User()
        {

        }

        // User using external login provider
        public User(string username, string email)
        {
            SetUsername(username);
            SetEmail(email);
            IsExternalLoginProvider = true;
            EmailConfirmed = true;
        }


        // User manually signing up
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
                throw new EmptyUsernameException();
            }

            if (UserName == username)
            {
                return;
            }

            if (username.Length < 4 || username.Length > 20)
            {
                throw new InvalidUsernameException();
            }

            UserName = username;
            NormalizedUserName = username.ToUpperInvariant();
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new EmptyEmailException();
            }

            if (Email == email)
            {
                return;
            }

            EmailAddressAttribute emailAddress = new EmailAddressAttribute();

            if (!emailAddress.IsValid(email))
            {
                throw new InvalidEmailException();
            }

            Email = email;
            NormalizedEmail = email.ToUpperInvariant();
        }

        public void SetPassword(string password)
        {
            if (PasswordHash == password)
                return;

            PasswordHash = password;
        }
    }
}
