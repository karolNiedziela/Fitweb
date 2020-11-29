using Backend.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Core.Domain
{
    public class UserExercise
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }

        public double Weight { get; set; }
        public int NumberOfSets { get; set; }
        public int NumberOfReps { get; set; }
        public Day Day { get; set; }

        public DateTime AddedAt { get; set; }

        public UserExercise()
        {

        }

        public UserExercise(User user, Exercise exercise, double weight, int numberOfSets,
            int numberOfReps, Day day)
        {
            User = user;
            Exercise = exercise;
            SetWeight(weight);
            SetNumberOfSets(numberOfSets);
            SetNumberOfReps(numberOfReps);
            Day = day;
            AddedAt = DateTime.Today;
        }

        public void SetWeight(double weight)
        {
            if (weight <= 0)
            {
                throw new DomainException(ErrorCodes.InvalidValue,
                    "Weight cannot be less than or equal to 0.");
            }

            Weight = weight;
        }

        public void SetNumberOfSets(int numberOfSets)
        {
            if (numberOfSets <= 0)
            {
                throw new DomainException(ErrorCodes.InvalidValue,
                    $"Number of sets cannot be less than or equal to 0.");
            }

            NumberOfSets = numberOfSets;
        }

        public void SetNumberOfReps(int numberOfReps)
        {
            if (numberOfReps <= 0)
            {
                throw new DomainException(ErrorCodes.InvalidValue,
                    $"Number of reps cannot be less than or equal to 0.");
            }

            NumberOfReps = numberOfReps;
        }

        public void SetDay(Day day)
        {
            Day = day;
        }

        public static UserExercise Create(User user, Exercise exercise, double weight,
            int numberOfSets, int numberOfReps, Day day)
            => new UserExercise(user, exercise, weight, numberOfSets, numberOfReps, day);
    }
}
