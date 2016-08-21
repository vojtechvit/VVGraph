using Domain.Model;
using System.Threading.Tasks;

namespace Domain.Repositories.Contracts
{
    public interface IGraphRepository
    {
        Task<Graph> GetAsync(string name);

        Task CreateAsync(Graph graph);

        Task DeleteAsync(string name);
    }
}