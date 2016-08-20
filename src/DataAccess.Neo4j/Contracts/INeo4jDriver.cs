using Neo4j.Driver.V1;

namespace DataAccess.Neo4j.Contracts
{
    public interface INeo4jDriver
    {
        ISession Session();
    }
}