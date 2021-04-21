using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Entities
{
    public class AthleteDietStats : BaseEntity
    {
        public Athlete Athlete { get; set; }

        public int UserId { get; set; }

        public DietStat DietStat { get; set; }

        public int DietStatId { get; set; }

        public AthleteDietStats()
        {

        }

        public AthleteDietStats(Athlete athlete, DietStat dietStat)
        {
            Athlete = athlete;
            DietStat = dietStat;
        }


        public static AthleteDietStats Create(Athlete athlete, DietStat dietStat) =>
            new AthleteDietStats(athlete, dietStat);
    }
}
