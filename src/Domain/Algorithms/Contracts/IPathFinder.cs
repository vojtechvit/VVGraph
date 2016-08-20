using Domain.Model;
using System.Threading.Tasks;

namespace Domain.Algorithms.Contracts
{
    public interface IPathFinder
    {
        Task<Path> GetShortestPathAsync(string graphName, int startNodeId, int endNodeId);
    }
}