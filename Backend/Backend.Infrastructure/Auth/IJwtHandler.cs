using Backend.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.Auth
{
    public interface IJwtHandler
    {
        JwtDto CreateToken(int userId, string username, string role);
    }
}
