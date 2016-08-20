using Domain.Validation;

namespace Domain.Validation.Contracts
{
    public interface IGraphValidator
    {
        ValidationResult ValidateGraphName(string name);
    }
}