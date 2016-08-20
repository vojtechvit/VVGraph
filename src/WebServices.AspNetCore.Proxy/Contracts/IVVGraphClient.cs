using System.Threading;
using System.Threading.Tasks;
using WebServices.ApiModel;

namespace WebServices.AspNetCore.Proxy.Contracts
{
    public interface IVVGraphClient
    {
        Task PutGraphAsync(Graph graph, CancellationToken cancellationToken);
    }
}