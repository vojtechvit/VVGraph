using Domain.Model;
using System.Collections.Generic;

namespace Domain.Factories.Contracts
{
    public interface INodeFactory
    {
        Node Create(string graphName, int id, string label, ISet<int> adjacentNodeIds);
    }
}