using Domain.Algorithms.Contracts;
using Domain.Model;
using System;
using System.Threading.Tasks;

namespace Domain.Algorithms
{
    public sealed class DummyPathFinder : IPathFinder
    {
        public Task<Path> FindShortestPathAsync(Graph graphName, Node startNode, Node endNode)
        {
            // We don't really use this implementation because we delegate the
            // pathfinding to neo4j.
            throw new NotImplementedException();
        }
    }
}