using System.Collections.Generic;
using WebServices.DataAccess.Neo4j.Model;

namespace WebServices.DataAccess.Neo4j.Mappers.Contracts
{
    public interface INeo4jGraphMapper
    {
        Domain.Model.Graph Map(Graph graph, IEnumerable<Node> nodes, IEnumerable<Edge> edges);

        Graph MapGraph(Domain.Model.Graph graph);

        IEnumerable<Node> MapNodes(Domain.Model.Graph graph);

        IEnumerable<Edge> MapEdges(Domain.Model.Graph graph);
    }
}