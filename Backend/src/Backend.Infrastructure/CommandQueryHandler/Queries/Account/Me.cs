using Backend.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Queries
{
    public class Me : IQuery<UserDto>
    { 
        public int UserId { get; set; }

        public Me(int userId)
        {
            UserId = userId;
        }
    }
}
