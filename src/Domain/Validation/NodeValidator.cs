using Domain.Validation.Contracts;

namespace Domain.Validation
{
    public sealed class NodeValidator : INodeValidator
    {
        public ValidationResult ValidateNodeId(int id)
        {
            var result = new ValidationResult();

            if (id <= 0)
            {
                result.AddError("Node id must be greater than 0.");
            }

            return result;
        }

        public ValidationResult ValidateNodeLabel(string label)
        {
            var result = new ValidationResult();

            if (label == null)
            {
                result.AddError("Node label is required.");
            }

            if (label == string.Empty)
            {
                result.AddError("Node label must not be empty.");
            }

            return result;
        }
    }
}