using Backend.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.DTO
{
    public class UserDetailsDto : UserDto
    {
        public IEnumerable<UserProductDto> Products { get; set; }
        public IEnumerable<UserExerciseDto> Exercises { get; set; }
    }
}
