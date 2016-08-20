using WebServices.ApiModel.Mappers.Contracts;

namespace WebServices.ApiModel.Mappers
{
    public sealed class EdgeMapper : IEdgeMapper
    {
        public Edge Map(Domain.Model.Edge edge)
            => new Edge
            {
                StartNodeId = edge.StartNode.Id,
                EndNodeId = edge.EndNode.Id
            };
    }
}