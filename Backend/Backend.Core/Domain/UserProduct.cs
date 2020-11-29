using Backend.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Core.Domain
{
    public class UserProduct
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public double Weight { get; set; } // in grams
        public double Calories { get; set; }
        public double Proteins { get; set; }
        public double Carbohydrates { get; set; }
        public double Fats { get; set; }

        public DateTime AddedAt { get; set; }


        public UserProduct()
        {

        }

        public UserProduct(User user, Product product, double weight)
        {
            User = user;
            Product = product;
            SetWeight(weight);
            AddedAt = DateTime.Today;

            AdjustCalories(weight, product);
        }
  

        public void SetWeight(double weight)
        {
            if (weight <= 0)
            {
                throw new DomainException(ErrorCodes.InvalidValue,
                    "Weight cannot be less than or equal to 0.");
            }

            Weight = weight;

        }

        public void AdjustCalories(double weight, Product product)
        {
            if (weight > 100)
            {
                Calories = (product.Calories * weight) / 100.0;
                Proteins = (product.Proteins * weight) / 100.0;
                Carbohydrates = (product.Carbohydrates * weight) / 100.0;
                Fats = (product.Fats * weight) / 100.0;
            }
            else
            {
                Calories = product.Calories * (weight / 100.0);
                Proteins = product.Proteins * (weight / 100.0);
                Carbohydrates = product.Carbohydrates * (weight / 100.0);
                Fats = product.Fats * (weight / 100.0);
            }
        }

        public static UserProduct Create(User user, Product product, double weight)
           => new UserProduct(user, product, weight);
    }
}
