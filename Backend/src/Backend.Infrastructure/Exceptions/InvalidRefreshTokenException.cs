using Backend.Infrastructure.Exceptions;

namespace Backend.Infrastructure.Exceptions
{
    public class InvalidRefreshTokenException : InfrastructureException
    {
        public InvalidRefreshTokenException() : base("Invalid refresh token.")
        {
        }
    }
}
