using Backend.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Core.Entities
{
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
                throw new DomainException(ErrorCodes.EmptyRefreshToken, "Empty refresh token.");
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
                throw new DomainException(ErrorCodes.RevokedRefreshToken, "Revoked refresh token");
            }

            RevokedAt = dateTime;
        }

        public void Revoke(DateTime revokedAt)
        {
            if (Revoked)
            {
                throw new DomainException(ErrorCodes.EmptyRefreshToken, "Empty refresh token");
            }

            RevokedAt = revokedAt;
        }
    }
}
