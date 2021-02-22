using Backend.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Core.Entities
{
    public class AthleteProduct : BaseEntity
    {
        public Athlete Athlete { get; set; }

        public int AthleteId { get; set; }

        public Product Product { get; set; }

        public int ProductId { get; set; }

        public double Weight { get; set; } // in grams

        public AthleteProduct()
        {

        }

        public AthleteProduct(Athlete athlete, Product product, double weight)
        {
            Athlete = athlete;
            Product = product;
            SetWeight(weight);
        }
  

        public void SetWeight(double weight)
        {
            if (weight <= 0)
            {
                throw new DomainException(ErrorCodes.InvalidValue,
                    "Weight cannot be less than or equal to 0.");
            }

            Weight = weight;
            DateUpdated = DateTime.UtcNow;
        }

       /* public void AdjustCalories(double weight, Product product)
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

            DateUpdated = DateTime.Now;
        }*/

        public static AthleteProduct Create(Athlete athlete, Product product, double weight)
           => new AthleteProduct(athlete, product, weight);
    }
}
