namespace DataLoader.Serialization.Contracts
{
    public interface IFileSystemGraphDeserializer
    {
        GraphDeserializationResult Deserialize(string graphName, string path);
    }
}