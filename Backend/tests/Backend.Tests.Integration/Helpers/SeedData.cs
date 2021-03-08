using Backend.Core.Entities;
using Backend.Infrastructure.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Tests.Integration.Helpers
{
    public static class SeedData
    {
        public static void AddProducts(FitwebContext ctx)
        {
            ctx.Products.AddRange(GetProducts());
            ctx.SaveChanges();
        }

        public static void RemoveProducts(FitwebContext ctx)
        {
            ctx.Products.RemoveRange(ctx.Products);
            AddProducts(ctx);
        }


        public static List<Product> GetProducts()
        {
            return new List<Product>()
            {
                new Product() { Name = "testProduct1", Calories = 100, Proteins = 10, Carbohydrates = 10, Fats = 10,
                CategoryOfProductId = 1},
                new Product() { Name = "testProduct2", Calories = 50, Proteins = 20, Carbohydrates = 15, Fats = 15,
                CategoryOfProductId = 1},
                new Product() { Name = "testProduct3", Calories = 100, Proteins = 10, Carbohydrates = 10, Fats = 10,
                CategoryOfProductId = 1},
            };
        }
    }
}
