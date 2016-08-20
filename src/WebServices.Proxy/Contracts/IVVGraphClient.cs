using System;
using System.Threading;
using System.Threading.Tasks;
using WebServices.ApiModel;

namespace WebServices.Proxy.Contracts
{
    public interface IVVGraphClient : IDisposable
    {
        Task PutGraphAsync(Graph graph, CancellationToken cancellationToken);
    }
}