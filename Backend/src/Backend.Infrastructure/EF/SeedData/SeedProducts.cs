using Backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.EF.SeedData
{
    public static class SeedProducts
    {
        public static List<Product> Get()
        {
            var p1 = new Product()
            {
                Id = 1,
                Name = "Banana",
                Calories = 95,
                Proteins = 0.3,
                Carbohydrates = 23.5,
                Fats = 0.3,
                CategoryOfProductId = 2
            };

            var p2 = new Product()
            {
                Id = 2,
                Name = "Peach",
                Calories = 46,
                Proteins = 1,
                Carbohydrates = 11.9,
                Fats = 0.2,
                CategoryOfProductId = 2
            };

            var p3 = new Product()
            {
                Id = 3,
                Name = "Pineapple",
                Calories = 54,
                Proteins = 0.4,
                Carbohydrates = 13.6,
                Fats = 0.2,
                CategoryOfProductId = 2
            };

            var p4 = new Product()
            {
                Id = 4,
                Name = "Apple",
                Calories = 36,
                Proteins = 0.4,
                Carbohydrates = 12.1,
                Fats = 0.4,
                CategoryOfProductId = 2
            };

            var p5 = new Product()
            {
                Id = 5,
                Name = "Chicken breast without skin",
                Calories = 99,
                Proteins = 21.5,
                Carbohydrates = 0,
                Fats = 1.3,
                CategoryOfProductId = 1
            };

            var p6 = new Product()
            {
                Id = 6,
                Name = "Veal shoulder",
                Calories = 106,
                Proteins = 19.9,
                Carbohydrates = 0,
                Fats = 2.8,
                CategoryOfProductId = 1
            };

            var p7 = new Product()
            {
                Id = 7,
                Name = "Pork ribs",
                Calories = 321,
                Proteins = 15.2,
                Carbohydrates = 0,
                Fats = 29.3,
                CategoryOfProductId = 1
            };

            var p8 = new Product()
            {
                Id = 8,
                Name = "Potatoes",
                Calories = 77,
                Proteins = 1.9,
                Carbohydrates = 18.3,
                Fats = 0.1,
                CategoryOfProductId = 3
            };

            var p9 = new Product()
            {
                Id = 9,
                Name = "Carrot",
                Calories = 27,
                Proteins = 1,
                Carbohydrates = 8.7,
                Fats = 0.2,
                CategoryOfProductId = 3
            };

            var p10 = new Product()
            {
                Id = 10,
                Name = "Lettuce",
                Calories = 14,
                Proteins = 1.4,
                Carbohydrates = 2.9,
                Fats = 0.2,
                CategoryOfProductId = 3
            };

            var list = new List<Product>
            {
                p1, p2, p3, p4, p5, p6, p7, p8, p9, p10
            };

            return list;
        }
    }
}
