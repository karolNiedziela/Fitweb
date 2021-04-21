using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Entities
{
    public class DietStat : BaseEntity
    {
        public double TotalCalories { get; set; } = 0;

        public double TotalProteins { get; set; } = 0;

        public double TotalCarbohydrates { get; set; } = 0;

        public double TotalFats { get; set; } = 0;


        public DietStat()
        {

        }

        public DietStat(double totalCalories, double totalProteins, double totalCarbohydrates, double totalFats)
        {
            TotalCalories = totalCalories;
            TotalProteins = totalProteins;
            TotalCarbohydrates = totalCarbohydrates;
            TotalFats = totalFats;
        }



    }
}
