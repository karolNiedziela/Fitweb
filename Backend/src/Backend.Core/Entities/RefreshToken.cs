﻿using Backend.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Core.Entities
{
    // Inspired by solution from https://github.com/spetz/workshops-asp/tree/master/src/Trill.Core 

    public class RefreshToken : BaseEntity
    {
        public int UserId { get; set; }

        public string Token { get; private set; }

        public DateTime? RevokedAt { get; private set; }

        public bool Revoked => RevokedAt.HasValue;

        public RefreshToken()
        {

        }

        public RefreshToken(int userId, string token, DateTime dateCreated, DateTime? revokedAt = null)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new EmptyRefreshTokenException();
            }

            UserId = userId;
            Token = token;
            DateCreated = dateCreated;
            RevokedAt = revokedAt;
        }

        public void Use(DateTime dateTime)
        {
            if (Revoked)
            {
                throw new RevokedRefreshTokenException();
            }

            RevokedAt = dateTime;
        }

        public void Revoke(DateTime revokedAt)
        {
            if (Revoked)
            {
                throw new RevokedRefreshTokenException();
            }

            RevokedAt = revokedAt;
        }
    }
}
