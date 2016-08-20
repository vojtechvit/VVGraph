using System.Collections.Generic;

namespace WebServices.ApiModel.Mappers.Contracts
{
    public interface INodeMapper
    {
        Node Map(Domain.Model.Node node);
    }
}