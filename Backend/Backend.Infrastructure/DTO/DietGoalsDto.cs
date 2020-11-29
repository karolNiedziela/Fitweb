using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.DTO
{
    public class DietGoalsDto
    {
        public double TotalCalories { get; set; }
        public double Proteins { get; set; }
        public double Carbohydrates { get; set; }
        public double Fats { get; set; }
        public int UserId { get; set; }
    }
}
