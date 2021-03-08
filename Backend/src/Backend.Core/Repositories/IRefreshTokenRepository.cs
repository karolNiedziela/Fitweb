using Backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetAsync(string token);

        Task AddAsync(RefreshToken refreshToken);

        Task UpdateAsync(RefreshToken refreshToken);
    }
}
