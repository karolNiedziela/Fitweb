using Backend.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Core.Entities
{
    public class AthleteExercise : BaseEntity
    {
        public Athlete Athlete { get; set; }

        public int AthleteId { get; set; }

        public Exercise Exercise { get; set; }

        public int ExerciseId { get; set; }

        public double Weight { get; set; }

        public int NumberOfSets { get; set; }

        public int NumberOfReps { get; set; }

        public int DayId { get; set; }

        public Day Day { get; set; }


        public AthleteExercise()
        {

        }

        public AthleteExercise(Athlete athlete, Exercise exercise, double weight, int numberOfSets,
            int numberOfReps, Day day)
        {
            Athlete = athlete;
            Exercise = exercise;
            SetWeight(weight);
            SetNumberOfSets(numberOfSets);
            SetNumberOfReps(numberOfReps);
            DayId = day.Id;
        }

        public void SetWeight(double weight)
        {
            if (weight <= 0)
            {
                throw new InvalidWeightException();
            }

            Weight = weight;
            DateUpdated = DateTime.Now;
        }

        public void SetNumberOfSets(int numberOfSets)
        {
            if (numberOfSets <= 0)
            {
                throw new InvalidNumberOfSetsException();
            }

            NumberOfSets = numberOfSets;
            DateUpdated = DateTime.Now;
        }

        public void SetNumberOfReps(int numberOfReps)
        {
            if (numberOfReps <= 0)
            {
                throw new InvalidNumberOfRepsException();
            }

            NumberOfReps = numberOfReps;
            DateUpdated = DateTime.Now;
        }

        public static AthleteExercise Create(Athlete athlete, Exercise exercise, double weight,
            int numberOfSets, int numberOfReps, Day day)
            => new AthleteExercise(athlete, exercise, weight, numberOfSets, numberOfReps, day);
    }
}
