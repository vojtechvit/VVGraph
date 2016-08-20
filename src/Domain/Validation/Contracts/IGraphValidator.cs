using Domain.Validation;

namespace Domain.Validation.Contracts
{
    public interface IGraphValidator
    {
        ValidationResult ValidateName(string name);
    }
}