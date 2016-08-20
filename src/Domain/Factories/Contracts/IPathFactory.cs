using Domain.Model;
using System.Collections.Generic;

namespace Domain.Factories.Contracts
{
    public interface IPathFactory
    {
        Path Create(IEnumerable<Node> nodes);
    }
}