using Backend.Core.Entities;
using Backend.Core.Repositories;
using Backend.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly FitwebContext _context;

        public RefreshTokenRepository(FitwebContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken> GetAsync(string token)
            => await _context.RefreshTokens.SingleOrDefaultAsync(rt => rt.Token == token);

        public async Task AddAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Update(refreshToken);
            await _context.SaveChangesAsync();
        }
    }
}
