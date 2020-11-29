using Backend.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.Services
{
    public interface IJwtHandler
    {
        JwtDto CreateToken(int userId, string role);
    }
}
