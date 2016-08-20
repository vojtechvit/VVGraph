using Domain.Model;
using System.Threading.Tasks;

namespace Domain.Algorithms.Contracts
{
    public interface IPathFinder
    {
        Task<Path> FindShortestPathAsync(Graph graphName, Node startNode, Node endNode);
    }
}