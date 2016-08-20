using System.Collections.Generic;

namespace WebServices.ApiModel.Mappers.Contracts
{
    public interface INodeMapper
    {
        IEnumerable<Domain.Model.Node> GetNodes(Graph graph);

        Node Map(Domain.Model.Node node);

        IEnumerable<Edge> GetEdges(IEnumerable<Domain.Model.Node> nodes);
    }
}