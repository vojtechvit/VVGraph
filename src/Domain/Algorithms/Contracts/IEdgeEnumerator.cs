using Domain.Model;
using System.Collections.Generic;

namespace Domain.Algorithms.Contracts
{
    public interface IEdgeEnumerator
    {
        IReadOnlyCollection<Edge> GetAllEdges(string graphName);
    }
}