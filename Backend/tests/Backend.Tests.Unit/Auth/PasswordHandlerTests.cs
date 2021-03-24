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
        public void IsValid_ShouldReturnTrue_WhenPasswordIsValid()
        {
            var password = "testPassword";
            var hash = _passwordHasher.HashPassword(Arg.Any<IPasswordHandler>(), password);

            var result = _sut.IsValid(hash, password);

            result.ShouldBeTrue();
        }

        [Fact]
        public void IsValid_ShouldReturnPasswordVerificationResultFailed_WhenPasswordIsNotValid()
        {
            var password = "testPassword";
            var hash = "testPass";

            var result = _sut.IsValid(hash, password);

            result.ShouldBeFalse();
        }
    }
}
