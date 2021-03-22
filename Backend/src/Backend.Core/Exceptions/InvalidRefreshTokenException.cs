namespace Backend.Core.Exceptions
{
    public class InvalidRefreshTokenException : CoreException
    {
        public InvalidRefreshTokenException() : base("Invalid refresh token.")
        {
        }
    }

}
