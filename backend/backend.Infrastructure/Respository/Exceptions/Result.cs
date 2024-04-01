using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public class Result<T>
    {
        public T Value { get; }
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }
        public string SuccessMessage { get; } // Dodane pole dla wiadomości o sukcesie
        public int StatusCode { get; }

        protected Result(T value, bool isSuccess, string errorMessage, string successMessage, int statusCode)
        {
            Value = value;
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            SuccessMessage = successMessage; // Inicjalizacja pola
            StatusCode = statusCode;
        }

        public static Result<T> Success(T value, string successMessage) => new Result<T>(value, true, null, successMessage, 200);

        public static Result<T> Failure(string errorMessage, int statusCode) => new Result<T>(default, false, errorMessage, null, statusCode);
    }


}
