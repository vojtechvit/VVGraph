using Domain.Model;

namespace Domain.Algorithms.Contracts
{
    public interface IPathFinder
    {
        Path GetShortestPath(string graphName, int startNodeId, int endNodeId);
    }
}