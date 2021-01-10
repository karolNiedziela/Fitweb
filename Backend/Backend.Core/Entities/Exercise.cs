using Backend.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Core.Entities
{
    public class Exercise : BaseEntity
    {

        public string Name { get; set; }

        public int PartOfBodyId { get; set; }

        public PartOfBody PartOfBody { get; set; }

        public ICollection<AthleteExercise> AthleteExercises { get; set; }

        public Exercise()
        {

        }

        public Exercise(string name, PartOfBody partOfBody)
        {
            SetName(name);
            PartOfBodyId = partOfBody.Id;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new DomainException(ErrorCodes.InvalidName, $"Name cannot be empty.");
            }

            Name = name;
            DateUpdated = DateTime.Now;
        }

        public static Exercise Create(string name, PartOfBody partOfBody)
            => new Exercise(name, partOfBody);
    }
}
