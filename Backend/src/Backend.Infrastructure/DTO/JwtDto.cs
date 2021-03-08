using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.DTO
{
    public class JwtDto
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public string Role { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public long Expires { get; set; }
    }
}
