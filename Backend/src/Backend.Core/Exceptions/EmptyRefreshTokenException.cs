namespace Backend.Core.Exceptions
{
    public class EmptyRefreshTokenException : CoreException
    {
        public EmptyRefreshTokenException() : base("Email refresh token.")
        {
        }
    }

}
