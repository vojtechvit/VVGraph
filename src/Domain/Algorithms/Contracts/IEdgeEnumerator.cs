using Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Algorithms.Contracts
{
    public interface IEdgeEnumerator
    {
        Task<IReadOnlyCollection<Edge>> GetAllEdgesAsync(string graphName);
    }
}