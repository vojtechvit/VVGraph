using System.Threading;
using System.Threading.Tasks;

namespace DataLoader.Contracts
{
    public interface IDataLoader
    {
        Task LoadAsync(
            string graphName,
            string directory,
            CancellationToken cancellationToken);
    }
}