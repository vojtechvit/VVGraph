using Domain.Model;
using Domain.Validation.Contracts;
using System;

namespace Domain.Validation
{
    public sealed class NodeValidator : INodeValidator
    {
        public ValidationResult ValidateId(int id)
        {
            var result = new ValidationResult();

            if (id <= 0)
            {
                result.AddError("Node id must be greater than 0.");
            }

            return result;
        }

        public ValidationResult ValidateLabel(string label)
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

        public ValidationResult ValidateBelongingToGraph(Graph graph, Node node)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var result = new ValidationResult();

            if (graph != node.Graph)
            {
                result.AddError($"The node with id '{node.Id}' belongs to graph '{node.Graph.Name}', but it should belong to graph '{graph.Name}' instead.");
            }

            return result;
        }
    }
}