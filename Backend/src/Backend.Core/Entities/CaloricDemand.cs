using Backend.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Entities
{
    public class CaloricDemand : BaseEntity
    {
        public double TotalCalories { get; set; }

        public double Proteins { get; set; }

        public double Carbohydrates { get; set; }

        public double Fats { get; set; }

        public Athlete Athlete { get; set; }

        public CaloricDemand()
        {

        }

        public CaloricDemand(double totalCalories, double proteins, double carbohydrates, double fats)
        {
            SetTotalCalories(totalCalories);
            SetProteins(proteins);
            SetCarbohydrates(carbohydrates);
            SetFats(fats);
        }

        public void SetTotalCalories(double totalCalories)
        {
            if (totalCalories <= 0)
            {
                throw new InvalidTotalCaloriesException();
            }

            TotalCalories = totalCalories;
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

        public static CaloricDemand Create(double totalCalories, double proteins, double carbohydrates, double fats) =>
            new CaloricDemand(totalCalories, proteins, carbohydrates, fats);
    }
}
