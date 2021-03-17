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

        public CaloricDemand()
        {

        }

        public CaloricDemand(double totalCalories, double proteins, double carbohydrates, double fats)
        {
            setTotalCalories(totalCalories);
            setProteins(proteins);
            setCarbohydrates(carbohydrates);
            setFats(fats);
        }

        public void setTotalCalories(double totalCalories)
        {
            if (totalCalories <= 0)
            {
                throw new DomainException(ErrorCodes.InvalidValue, $"Total calories cannot be less or equal to 0");
            }

            TotalCalories = totalCalories;
            DateUpdated = DateTime.UtcNow;
        }

        public void setProteins(double proteins)
        {
            if (proteins < 0)
            {
                throw new DomainException(ErrorCodes.InvalidValue, $"Proteins cannot be less or equal to 0");
            }

            Proteins = proteins;
            DateUpdated = DateTime.UtcNow;
        }

        public void setCarbohydrates(double carbohydrates)
        {
            if (carbohydrates < 0)
            {
                throw new DomainException(ErrorCodes.InvalidValue, $"Carbohydrates cannot be less or equal to 0");
            }

            Carbohydrates = carbohydrates;
            DateUpdated = DateTime.UtcNow;
        }

        public void setFats(double fats)
        {
            if (fats < 0)
            {
                throw new DomainException(ErrorCodes.InvalidValue, $"Fats cannot be less or equal to 0");
            }

            Fats = fats;
            DateUpdated = DateTime.UtcNow;
        }

        public static CaloricDemand Create(double totalCalories, double proteins, double carbohydrates, double fats) =>
            new CaloricDemand(totalCalories, proteins, carbohydrates, fats);
    }
}
