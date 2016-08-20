using System.Threading;
using System.Threading.Tasks;
using WebServices.ApiModel;

namespace WebServices.AspNetCore.Proxy.Contracts
{
    public interface IVVGraphClient
    {
        Task DeleteGraphAsync(string graphName, CancellationToken cancellationToken);

        Task CreateGraphAsync(Graph graph, CancellationToken cancellationToken);
    }
}