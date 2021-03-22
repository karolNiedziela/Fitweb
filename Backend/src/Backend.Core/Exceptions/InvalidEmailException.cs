namespace Backend.Core.Exceptions
{
    public class InvalidEmailException : CoreException
    {
        public InvalidEmailException() : base("Invalid email format.")
        {
        }
    }
}
