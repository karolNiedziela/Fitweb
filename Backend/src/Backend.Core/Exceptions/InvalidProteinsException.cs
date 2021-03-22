namespace Backend.Core.Exceptions
{
    public class InvalidProteinsException : CoreException
    {
        public InvalidProteinsException() : base("Proteins cannot be less than 0.")
        {
        }
    }
}
