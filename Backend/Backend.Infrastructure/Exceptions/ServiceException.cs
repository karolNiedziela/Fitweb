using Backend.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.Exceptions
{
    public class ServiceException : CustomException
    {
        protected ServiceException()
        {

        }

        public ServiceException(string code)
            : base(code)
        {
        }

        public ServiceException(string message, params object[] args)
            : base(message, args)
        {

        }

        public ServiceException(string code, string message, params object[] args)
            : base(null, code, message, args)
        {

        }

        public ServiceException(Exception innerException, string message, params object[] args)
            : base(innerException, string.Empty, message, args)
        {

        }

        public ServiceException(Exception innerException, string code, string message, params object[] args)
            : base(code, string.Format(message, args), innerException)
        {
            
        }
    }
}
