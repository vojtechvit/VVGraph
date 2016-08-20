using System;

namespace Domain.Validation
{
    public sealed class ValidationError
    {
        internal ValidationError(string message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            Message = message;
        }

        public string Message { get; }
    }
}