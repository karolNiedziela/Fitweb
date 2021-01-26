using Backend.Core.Entities;
using Backend.Core.Exceptions;
using Backend.Infrastructure.Auth;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Unit.Auth
{
    public class PasswordHandlerTests
    {
        private readonly IPasswordHasher<IPasswordHandler> _passwordHasher;
        private readonly IPasswordHandler _passwordHandler;

        public PasswordHandlerTests()
        {
            _passwordHasher = new PasswordHasher<IPasswordHandler>();
            _passwordHandler = Substitute.For<PasswordHandler>(_passwordHasher);
                  
        }

        [Fact]
        public void Hash_ShouldReturnHashedPassword()
        {
            var password = "testPassword";
            
            var hash = _passwordHandler.Hash(password);

            hash.ShouldNotBeEmpty();
            hash.ShouldBeOfType<string>();
        }

        [Fact]
        public void Hash_ShouldReturnException_WhenPasswordIsNull()
        {
            var password = string.Empty;

            var exception = Record.Exception(() => _passwordHandler.Hash(password));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(DomainException));
            exception.ShouldBeOfType(typeof(DomainException), "Password cannot be empty");
        }

        [Fact]
        public void Hash_ShouldReturnException_WhenPasswordIsShortherThan4Characters()
        {
            var password = "pas";

            var exception = Record.Exception(() => _passwordHandler.Hash(password));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(DomainException));
            exception.ShouldBeOfType(typeof(DomainException), "Password cannot contain less than 4 characters.");
        }

        [Fact]
        public void Hash_ShouldReturnException_WhenPasswordIsShortherThan40Characters()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var password = string.Join("", Enumerable.Repeat(chars, 45).Select(s => s[random.Next(s.Length)]));

            var exception = Record.Exception(() => _passwordHandler.Hash(password));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(DomainException));
            exception.ShouldBeOfType(typeof(DomainException), "Password cannot contain more than 40 characters.");
        }

        [Fact]
        public void IsValid_ShouldReturnTrue_WhenPasswordIsValid()
        {
            var password = "secret";
            var hash = _passwordHandler.Hash(password);

            var result = _passwordHandler.IsValid(hash, password);

            result.ShouldBeTrue();
        }

        [Fact]
        public void IsValid_ShouldReturnPasswordVerificationResultFailed_WhenPasswordIsNotValid()
        {
            var password = "secret";
            var hash = _passwordHandler.Hash("secre");

            var result = _passwordHandler.IsValid(hash, password);

            result.ShouldBeFalse();
        }
    }
}
