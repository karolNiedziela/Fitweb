namespace Backend.Infrastructure.Exceptions
{
    public class InvalidFacebookTokenException : InfrastructureException
    {
        public InvalidFacebookTokenException() : base("Invalid facebook token.")
        {

        }
    }

}
