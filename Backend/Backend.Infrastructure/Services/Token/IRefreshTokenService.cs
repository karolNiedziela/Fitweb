using Backend.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public interface IRefreshTokenService
    {
        Task<JwtDto> UseAsync(string refreshToken);

        Task RevokeAsync(string refreshToken);
    }
}
