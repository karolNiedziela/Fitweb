using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.DTO
{
    public class DietStatDto
    {
        public double TotalCalories { get; set; }

        public double TotalProteins { get; set; }

        public double TotalCarbohydrates { get; set; }

        public double TotalFats { get; set; }

        public string DateCreated { get; set; }

    }
}
