using Domain.Validation.Contracts;
using System.Text.RegularExpressions;

namespace Domain.Validation
{
    public sealed class GraphValidator : IGraphValidator
    {
        public ValidationResult ValidateGraphName(string name)
        {
            var result = new ValidationResult();

            if (name == null)
            {
                result.AddError("Graph name is required.");
                return result;
            }

            if (name.Length <= 0)
            {
                result.AddError("Graph name must be at least 1 character long.");
            }

            if (!Regex.IsMatch(name, "[a-z0-9]"))
            {
                result.AddError("Graph name may contain only lowercase alphanumeric characters.");
            }

            return result;
        }
    }
}