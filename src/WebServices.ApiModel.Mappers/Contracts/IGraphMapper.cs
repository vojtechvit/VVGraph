using System.Collections.Generic;

namespace WebServices.ApiModel.Mappers.Contracts
{
    public interface IGraphMapper
    {
        Domain.Model.Graph Map(Graph graph);

        Graph Map(Domain.Model.Graph graph, IEnumerable<Domain.Model.Node> nodes);
    }
}