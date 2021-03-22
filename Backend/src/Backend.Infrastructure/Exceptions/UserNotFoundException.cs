namespace Backend.Infrastructure.Exceptions
{
    public class UserNotFoundException : InfrastructureException
    {
        public UserNotFoundException(int id) : base($"User with id: '{id}' was not found.")
        {

        }
    }
}
