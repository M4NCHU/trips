using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public class CategoryProcessingException : Exception
    {
        public int StatusCode { get; }

        public CategoryProcessingException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
