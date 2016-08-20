using System.Collections.Generic;

namespace Domain.Validation
{
    public sealed class ValidationResult
    {
        private readonly IList<ValidationError> _errors = new List<ValidationError>();

        internal ValidationResult()
        {
        }

        public bool IsValid => _errors.Count == 0;

        public IEnumerable<ValidationError> Errors => _errors;

        public void AddError(string message)
        {
            _errors.Add(new ValidationError(message));
        }
    }
}