using Backend.Core.Domain;
using Backend.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Fitweb.Tests.Domain
{
    public class ProductTests
    {
        [Theory]
        [InlineData("product1")]
        [InlineData("banan")]
        public void SetName_ShouldWork(string name)
        {
            var product = new Product { Name = "product" };

            product.SetName(name);

            Assert.Equal(name, product.Name);
        }

        [Theory]
        [InlineData("")]
        public void SetName_ShouldFail(string name)
        {
            var product = new Product { Name = "product" };

            Assert.Throws<DomainException>(() => product.SetName(name));
        }

        [Theory]
        [InlineData(100)]
        public void SetCalories_ShouldWork(double calories)
        {
            var product = new Product { Calories = 200 };

            product.SetCalories(calories);

            Assert.Equal(calories, product.Calories);
        }
    }
}
