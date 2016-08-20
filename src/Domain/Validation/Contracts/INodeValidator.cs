namespace Domain.Validation.Contracts
{
    public interface INodeValidator
    {
        ValidationResult ValidateId(int id);

        ValidationResult ValidateLabel(string label);

        ValidationResult ValidateAdjacency(int startNodeId, int endNodeId);
    }
}