using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Commands.Athletes
{
    public class UpdateCaloricDemand : ICommand
    {
        public int UserId { get; set; }

        public double TotalCalories { get; set; }

        public double Proteins { get; set; }

        public double Carbohydrates { get; set; }

        public double Fats { get; set; }
    }
}
