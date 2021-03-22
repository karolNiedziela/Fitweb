using Backend.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        public double Calories { get; set; }

        public double Proteins { get; set; }

        public double Carbohydrates { get; set; }

        public double Fats { get; set; }

        public ICollection<AthleteProduct> AthleteProducts { get; set; }

        public int CategoryOfProductId { get; set; }

        public CategoryOfProduct CategoryOfProduct { get; set; }

        public Product()
        {

        }

        public Product(string name, double calories, double proteins, double carbohydrates, double fats, CategoryOfProduct categoryOfProduct)
        {
            SetName(name);
            SetCalories(calories);
            SetProteins(proteins);
            SetCarbohydrates(carbohydrates);
            SetFats(fats);
            CategoryOfProductId = categoryOfProduct.Id;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidNameException();
            }

            Name = name;
            DateUpdated = DateTime.UtcNow;
        }

        public void SetCalories(double calories)
        {
            if (calories < 0)
            {
                throw new InvalidCaloriesException();
            }

            Calories = calories;
            DateUpdated = DateTime.UtcNow;
        }

        public void SetProteins(double proteins)
        {
            if (proteins < 0)
            {
                throw new InvalidProteinsException();
            }

            Proteins = proteins;
            DateUpdated = DateTime.UtcNow;
        }

        public void SetCarbohydrates(double carbohydrates)
        {
            if (carbohydrates < 0)
            {
                throw new InvalidCarbohydratesException();
            }

            Carbohydrates = carbohydrates;
            DateUpdated = DateTime.UtcNow;
        }

        public void SetFats(double fats)
        {
            if (fats < 0)
            {
                throw new InvalidFatsException();
            }

            Fats = fats;
            DateUpdated = DateTime.UtcNow;
        }

        public static Product Create(string name, double calories, double proteins,
            double carbohydrates, double fats, CategoryOfProduct categoryOfProduct)
            => new Product(name, calories, proteins, carbohydrates, fats, categoryOfProduct);
    }
}
