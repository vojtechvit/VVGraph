using WebServices.ApiModel.Mappers.Contracts;

namespace WebServices.ApiModel.Mappers
{
    public sealed class ApiModelNodeMapper : IApiModelNodeMapper
    {
        public Node Map(Domain.Model.Node node)
            => new Node { Id = node.Id, Label = node.Label };
    }
}