using Domain.Model;
using System.Collections.Generic;

namespace Domain.Factories.Contracts
{
    public interface IGraphFactory
    {
        Graph Create(string name);
    }
}