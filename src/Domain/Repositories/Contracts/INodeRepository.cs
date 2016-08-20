using Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repositories.Contracts
{
    public interface INodeRepository
    {
        Task<IReadOnlyCollection<Node>> GetAllNodesForGraphAsync(string graphName);

        Task CreateAllAsync(IEnumerable<Node> node);

        Task DeleteAllForGraphAsync(string graphName);
    }
}