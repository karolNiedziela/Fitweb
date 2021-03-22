namespace Backend.Core.Exceptions
{
    public class RevokedRefreshTokenException : CoreException
    {
        public RevokedRefreshTokenException() : base("Revoked refresh token.")
        {
        }
    }
}
