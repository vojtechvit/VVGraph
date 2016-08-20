using Domain.Model;
using System.Collections.Generic;

namespace Domain.Factories.Contracts
{
    public interface IPathFactory
    {
        Path Create(string graphName, IEnumerable<int> nodeIds);
    }
}