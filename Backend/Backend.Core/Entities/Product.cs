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
                throw new DomainException(ErrorCodes.InvalidName, "Name cannot be empty.");
            }

            Name = name;
            DateUpdated = DateTime.Now;
        }

        public void SetCalories(double calories)
        {
            if (calories < 0)
            {
                throw new DomainException(ErrorCodes.InvalidValue, "Calories cannot be less than 0.");
            }

            Calories = calories;
            DateUpdated = DateTime.Now;
        }

        public void SetProteins(double proteins)
        {
            if (proteins < 0)
            {
                throw new DomainException(ErrorCodes.InvalidValue, "Proteins cannot be less than 0.");
            }

            Proteins = proteins;
            DateUpdated = DateTime.Now;
        }

        public void SetCarbohydrates(double carbohydrates)
        {
            if (carbohydrates < 0)
            {
                throw new DomainException(ErrorCodes.InvalidValue, "Carbohydrates cannot be less than 0.");
            }

            Carbohydrates = carbohydrates;
            DateUpdated = DateTime.Now;
        }

        public void SetFats(double fats)
        {
            if (fats < 0)
            {
                throw new DomainException(ErrorCodes.InvalidValue, "Fats cannot be less than 0.");
            }

            Fats = fats;
            DateUpdated = DateTime.Now;
        }

        public static Product Create(string name, double calories, double proteins,
            double carbohydrates, double fats, CategoryOfProduct categoryOfProduct)
            => new Product(name, calories, proteins, carbohydrates, fats, categoryOfProduct);
    }
}
