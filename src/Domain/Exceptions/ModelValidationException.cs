using System;

namespace Domain.Exceptions
{
    public sealed class ModelValidationException : Exception
    {
        public ModelValidationException()
        {
        }

        public ModelValidationException(string message) : base(message)
        {
        }

        public ModelValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}