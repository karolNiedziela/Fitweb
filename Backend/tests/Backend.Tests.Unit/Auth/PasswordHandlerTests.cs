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
        private readonly PasswordHandler _sut;

        public PasswordHandlerTests()
        {
            _passwordHasher = new PasswordHasher<IPasswordHandler>();
            _sut = Substitute.For<PasswordHandler>(_passwordHasher);
                  
        }

        [Fact]
        public void Hash_ShouldReturnHashedPassword()
        {
            var password = "testPassword";
            
            var hash = _sut.Hash(password);

            hash.ShouldNotBeEmpty();
            hash.ShouldBeOfType<string>();
        }

        [Fact]
        public void Hash_ShouldThrowException_WhenPasswordIsNull()
        {
            var password = string.Empty;

            var exception = Record.Exception(() => _sut.Hash(password));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(EmptyPasswordException));
            exception.Message.ShouldBe("Password cannot be empty.");
        }

        [Fact]
        public void Hash_ShouldThrowException_WhenPasswordIsShortherThan4Characters()
        {
            var password = "pas";

            var exception = Record.Exception(() => _sut.Hash(password));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(InvalidPasswordException));
            exception.Message.ShouldBe("Password cannot contain less than 4 characters.");
        }

        [Fact]
        public void Hash_ShouldReturnException_WhenPasswordIsShortherThan40Characters()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var password = string.Join("", Enumerable.Repeat(chars, 45).Select(s => s[random.Next(s.Length)]));

            var exception = Record.Exception(() => _sut.Hash(password));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(InvalidPasswordException));
            exception.Message.ShouldBe("Password cannot contain more than 40 characters.");
        }

        [Fact]
        public void IsValid_ShouldReturnTrue_WhenPasswordIsValid()
        {
            var password = "secret";
            var hash = _sut.Hash(password);

            var result = _sut.IsValid(hash, password);

            result.ShouldBeTrue();
        }

        [Fact]
        public void IsValid_ShouldReturnPasswordVerificationResultFailed_WhenPasswordIsNotValid()
        {
            var password = "secret";
            var hash = _sut.Hash("secre");

            var result = _sut.IsValid(hash, password);

            result.ShouldBeFalse();
        }
    }
}
