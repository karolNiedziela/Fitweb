using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Core.Exceptions
{
    public abstract class CoreException : Exception
    {
        protected CoreException(string message) : base(message)
        {

        }
    }
}
