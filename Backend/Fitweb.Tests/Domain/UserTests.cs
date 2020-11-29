using Backend.Core.Domain;
using Backend.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Fitweb.Tests.Domain
{
    public class UserTests
    {
        [Theory]
        [InlineData("Karol")]
        [InlineData("Mateusz")]
        public void SetUsername_ShouldWork(string username)
        {
            var user = new User { Username = "user1" };

            user.SetUsername(username);

            Assert.Equal(username.ToLowerInvariant(), user.Username);
        }

        [Theory]
        [InlineData("")]
        public void SetUsername_ShouldFail(string username)
        {
            var user = new User { Username = "user1" };

            Assert.Throws<DomainException>(() => user.SetUsername(username));
        }

        [Theory]
        [InlineData("email@email.com")]
        [InlineData("karol@email.com")]
        public void SetEmail_ShouldWork(string email)
        {
            var user = new User { Email = "email@email.com" };

            user.SetEmail(email);

            Assert.Equal(email.ToLowerInvariant(), user.Email);
        }

        [Theory]
        [InlineData("")]
        public void SetEmail_ShouldFail(string email)
        {
            var user = new User { Email = "user1@email.com" };

            Assert.Throws<DomainException>(() => user.SetEmail(email));
        }
    }
}
