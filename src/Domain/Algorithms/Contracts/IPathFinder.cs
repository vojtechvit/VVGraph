using Domain.Model;

namespace Domain.Algorithms.Contracts
{
    public interface IPathFinder
    {
        Path GetShortestPath(Graph graph, Node startNode, Node endNode);
    }
}