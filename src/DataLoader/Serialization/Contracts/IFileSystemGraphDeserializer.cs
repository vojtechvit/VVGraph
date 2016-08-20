using Domain.Model;

namespace DataLoader.Serialization.Contracts
{
    public interface IFileSystemGraphDeserializer
    {
        Graph Deserialize(string graphName, string path);
    }
}