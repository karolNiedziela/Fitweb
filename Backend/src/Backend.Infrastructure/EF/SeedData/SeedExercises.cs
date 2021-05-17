using Backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.EF.SeedData
{
    public static class SeedExercises
    {

        public static List<Exercise> Get()
        {
            var e1 = new Exercise
            {
                Id = 1,
                Name = "Barbell Chest Press",
                PartOfBodyId = 1
            };

            var e2 = new Exercise
            {
                Id = 2,
                Name = "Dumbbell Chest Press",
                PartOfBodyId = 1
            };

            var e3 = new Exercise
            {
                Id = 3,
                Name = "Wide-Grip Chest Press",
                PartOfBodyId = 1
            };

            var e4 = new Exercise
            {
                Id = 4,
                Name = "Push-Ups",
                PartOfBodyId = 1
            };

            var e5 = new Exercise
            {
                Id = 5,
                Name = "Squat",
                PartOfBodyId = 2
            };

            var e6 = new Exercise
            {
                Id = 6,
                Name = "Leg Curl",
                PartOfBodyId = 2
            };

            var e7 = new Exercise
            {
                Id = 7,
                Name = "Leg Extension",
                PartOfBodyId = 2
            };

            var e8 = new Exercise
            {
                Id = 8,
                Name = "Standinag Dumbbell Curl",
                PartOfBodyId = 3
            };

            var e9 = new Exercise
            {
                Id = 9,
                Name = "Hammer Curl",
                PartOfBodyId = 3
            };

            var e10 = new Exercise
            {
                Id = 10,
                Name = "Close-Grip Bench Press",
                PartOfBodyId = 4
            };

            var e11 = new Exercise
            {
                Id = 11,
                Name = "Cable Rope Triceps Pushdown",
                PartOfBodyId = 4
            };

            var list = new List<Exercise>
            {
                e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11
            };

            return list;
        }
    }
}
