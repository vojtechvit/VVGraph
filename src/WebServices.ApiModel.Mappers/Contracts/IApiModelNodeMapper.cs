using System.Collections.Generic;

namespace WebServices.ApiModel.Mappers.Contracts
{
    public interface IApiModelNodeMapper
    {
        Node Map(Domain.Model.Node node);
    }
}