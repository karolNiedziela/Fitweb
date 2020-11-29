using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.Commands
{
    public class AddDietGoals : AuthenticatedCommandBase
    {
        public double TotalCalories { get; set; }
        public double Proteins { get; set; }
        public double Carbohydrates { get; set; }
        public double Fats { get; set; }
    }
}
