using Domain.Model;

namespace Domain.Repositories.Contracts
{
    public interface IGraphRepository
    {
        bool Exists();

        Graph Get(string name);

        void CreateOrReplace(Graph graph);
    }
}