namespace Backend.Core.Exceptions
{
    public class InvalidPasswordException : CoreException
    {
        public InvalidPasswordException() : base("Password cannot contain less than 6 characters and " +
                    "more than 20 characters.")
        {
        }
    }
}
