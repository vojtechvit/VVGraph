using System;
using System.Threading;
using System.Threading.Tasks;

namespace DataLoader.Contracts
{
    public interface IDataLoader
    {
        Task LoadAsync(
            Uri baseUrl,
            string graphName,
            string directory,
            CancellationToken cancellationToken);
    }
}