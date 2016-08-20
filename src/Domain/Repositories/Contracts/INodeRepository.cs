using Domain.Model;
using System.Collections.Generic;

namespace Domain.Repositories.Contracts
{
    public interface INodeRepository
    {
        IReadOnlyCollection<Node> GetAllNodesForGraph(string graphName);

        void CreateAll(IEnumerable<Node> node);

        void DeleteAllForGraph(string graphName);
    }
}