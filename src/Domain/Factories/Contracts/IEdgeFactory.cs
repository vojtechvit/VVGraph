using Domain.Model;

namespace Domain.Factories.Contracts
{
    public interface IEdgeFactory
    {
        Edge Create(string graphName, int startNodeId, int endNodeId);
    }
}