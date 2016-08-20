using Domain.Model;

namespace Domain.Validation.Contracts
{
    public interface INodeValidator
    {
        ValidationResult ValidateId(int id);

        ValidationResult ValidateLabel(string label);

        ValidationResult ValidateBelongingToGraph(Graph graph, Node node);
    }
}