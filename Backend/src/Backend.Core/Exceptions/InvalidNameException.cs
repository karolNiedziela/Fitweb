namespace Backend.Core.Exceptions
{
    public class InvalidNameException : CoreException
    {
        public InvalidNameException() : base("Name cannot be empty.")
        {
        }
    }

}
