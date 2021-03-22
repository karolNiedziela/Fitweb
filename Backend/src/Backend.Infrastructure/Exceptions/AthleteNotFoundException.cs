namespace Backend.Infrastructure.Exceptions
{
    public class AthleteNotFoundException : InfrastructureException
    {
        public AthleteNotFoundException(int userId) : base($"Athlete with user id: '{userId}' was not found.")
        {
        }
    }
}
