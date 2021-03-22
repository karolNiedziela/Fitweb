namespace Backend.Core.Exceptions
{
    public class InvalidPasswordException : CoreException
    {
        public InvalidPasswordException(string message) : base(message)
        {
        }
    }
}
