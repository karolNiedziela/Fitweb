﻿using Backend.Core.Factories;
using Backend.Core.Repositories;
using Backend.Core.Services;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IRefreshTokenFactory _refreshTokenFactory;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IJwtHandler _jwtHandler;
        private readonly IUserService _userService;

        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository, IUserService userService,
            IRefreshTokenFactory refreshTokenFactory, IDateTimeProvider dateTimeProvider, IJwtHandler jwtHandler)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _refreshTokenFactory = refreshTokenFactory;
            _dateTimeProvider = dateTimeProvider;
            _jwtHandler = jwtHandler;
            _userService = userService;
        }

        public async Task<JwtDto> UseAsync(string refreshToken)
        {
            var token = await _refreshTokenRepository.GetAsync(refreshToken);
            if (token is null)
            {
                throw new ServiceException(ErrorCodes.InvalidRefreshToken, "Invalid refresh token.");
            }

            token.Use(_dateTimeProvider.Now);
            var user = await _userService.GetAsync(token.UserId);
            var jwt = _jwtHandler.CreateToken(token.UserId, user.Username, user.Role);
            var newRefreshToken = _refreshTokenFactory.Create(user.Id);
            await _refreshTokenRepository.AddAsync(newRefreshToken);
            await _refreshTokenRepository.UpdateAsync(token);
            jwt.RefreshToken = newRefreshToken.Token;

            return jwt;

        }

        public async Task RevokeAsync(string refreshToken)
        {
            var token = await _refreshTokenRepository.GetAsync(refreshToken);
            if (token is null)
            {
                throw new ServiceException(ErrorCodes.InvalidRefreshToken, "Invalid refresh token.");
            }

            token.Revoke(_dateTimeProvider.Now);
            await _refreshTokenRepository.UpdateAsync(token);
        }    
    }
}
