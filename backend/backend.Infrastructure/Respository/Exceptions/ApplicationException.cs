using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public abstract class ApplicationException : Exception
    {
        public int StatusCode { get; }

        protected ApplicationException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        public class NotFoundException : ApplicationException
        {
            public NotFoundException(string message) : base(message, 404)
            {
            }
        }

        public class ValidationException : ApplicationException
        {
            public ValidationException(string message) : base(message, 400)
            {
            }
        }

        public class InternalErrorException : ApplicationException
        {
            public InternalErrorException(string message) : base(message, 500)
            {
            }
        }

        // Możesz dodać więcej wyjątków specyficznych dla różnych przypadków.

    }

}
