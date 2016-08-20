namespace Domain.Validation.Contracts
{
    public interface INodeValidator
    {
        ValidationResult ValidateNodeId(int id);

        ValidationResult ValidateNodeLabel(string label);
    }
}