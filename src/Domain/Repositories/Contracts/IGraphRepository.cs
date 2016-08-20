using Domain.Model;

namespace Domain.Repositories.Contracts
{
    public interface IGraphRepository
    {
        bool Exists(string name);

        Graph Get(string name);

        void Create(Graph graph);

        void Delete(string name);
    }
}