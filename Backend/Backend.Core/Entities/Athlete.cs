using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Core.Entities
{
    public class Athlete : BaseEntity
    {
        public User User { get; set; }

        public int UserId { get; set; }

        public ICollection<AthleteProduct> AthleteProducts { get; set; }

        public ICollection<AthleteExercise> AthleteExercises { get; set; }

        public Athlete()
        {

        }

        public Athlete(User user)
        {
            UserId = user.Id;
        }
    }
}
