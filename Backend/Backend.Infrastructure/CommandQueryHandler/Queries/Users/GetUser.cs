using Backend.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Queries.Users
{
    public class GetUser : IQuery<UserDto>
    {
        public int Id { get; set; }

        public GetUser(int id)
        {
            Id = id;
        }
    }
}
