using Backend.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Queries.Users
{
    public class GetUsers : IQuery<IEnumerable<UserDto>>
    {
    }
}
