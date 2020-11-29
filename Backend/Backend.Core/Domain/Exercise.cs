using Backend.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Core.Domain
{
    public class Exercise
    {
        public int Id { get; set; }
        public string PartOfBody { get; set; }
        public string Name { get; set; }

        public IEnumerable<UserExercise> UserExercises { get; protected set; }

        public Exercise()
        {

        }

        public Exercise(string partOfBody, string name)
        {
            SetPartOfBody(partOfBody);
            SetName(name);
        }

        public void SetPartOfBody(string partOfBody)
        {
            if (string.IsNullOrEmpty(partOfBody))
            {
                throw new DomainException(ErrorCodes.InvalidName, $"Part of body cannot be empty.");
            }

            PartOfBody = partOfBody;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new DomainException(ErrorCodes.InvalidName, $"Name cannot be empty.");
            }

            Name = name;
        }
    }
}
