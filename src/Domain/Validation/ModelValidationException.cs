using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Validation
{
    public sealed class ModelValidationException : Exception
    {
        public ModelValidationException(string message)
            : base(message)
        {
            ValidationErrors = new[] { new ValidationError(message) };
        }

        public ModelValidationException(IEnumerable<ValidationError> validationErrors)
            : base(string.Join(" ", validationErrors.Select(e => e.Message)))
        {
            ValidationErrors = validationErrors;
        }

        public IEnumerable<ValidationError> ValidationErrors { get; }
    }
}