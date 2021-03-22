namespace Backend.Core.Exceptions
{
    public class EmptyPasswordException : CoreException
    {
        public EmptyPasswordException() : base("Password cannot be empty.")
        {

        }
    }
}
