using Backend.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Core.Domain
{
    public class DietGoals
    {
        public int Id { get; set; }
        public double TotalCalories { get; set; }
        public double Proteins { get; set; }
        public double Carbohydrates { get; set; }
        public double Fats { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }

        protected DietGoals()
        {

        }

        public DietGoals(double totalCalories, double proteins, double carbohydrates, double fats, int userId)
        {
            SetTotalCalories(totalCalories);
            SetProteins(proteins);
            SetCarbohydrates(carbohydrates);
            SetFats(fats);
            UserId = userId;
        }

        public void SetTotalCalories(double totalCalories)
        {
            if (totalCalories <= 0 )
            {
                throw new DomainException(ErrorCodes.InvalidEmail, "Total calories cannot be less or equal to 0.");
            }

            TotalCalories = totalCalories;
        }

        public void SetProteins(double proteins)
        {
            if (proteins <= 0)
            {
                throw new DomainException(ErrorCodes.InvalidEmail, "Proteins cannot be less or equal to 0.");
            }

            Proteins = proteins;
        }

        public void SetCarbohydrates(double carbohydrates)
        {
            if (carbohydrates <= 0)
            {
                throw new DomainException(ErrorCodes.InvalidEmail, "Carbohydrates cannot be less or equal to 0.");
            }

            Carbohydrates = carbohydrates;
        }

        public void SetFats(double fats)
        {
            if (fats <= 0)
            {
                throw new DomainException(ErrorCodes.InvalidEmail, "Fats cannot be less or equal to 0.");
            }

            Fats = fats;
        }

        public static DietGoals Create(double totalCalories, double proteins,
            double carbohydrates, double fats, int userId)
            => new DietGoals(totalCalories, proteins, carbohydrates, fats, userId);
    }
}
