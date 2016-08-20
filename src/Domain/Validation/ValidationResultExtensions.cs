using System;

namespace Domain.Validation
{
    public static class ValidationResultExtensions
    {
        public static void ThrowIfInvalid(this ValidationResult validationResult)
        {
            if (validationResult == null)
                throw new ArgumentNullException(nameof(validationResult));

            if (!validationResult.IsValid)
            {
                throw new ModelValidationException(validationResult.Errors);
            }
        }
    }
}