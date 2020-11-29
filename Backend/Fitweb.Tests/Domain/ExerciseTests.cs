using Backend.Core.Domain;
using Backend.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Fitweb.Tests.Domain
{
    public class ExerciseTests
    {
        [Theory]
        [InlineData("exercise")]
        [InlineData("bench press")]
        public void SetName_ShouldWork(string name)
        {
            var exercise = new Exercise { Name = "something" };

            exercise.SetName(name);

            Assert.Equal(name, exercise.Name);
        }

        [Theory]
        [InlineData("")]
        public void SetName_ShouldFail(string name)
        {
            var exercise = new Exercise { Name = "something" };

            Assert.Throws<DomainException>(() => exercise.SetName(name));
        }
    }
}
