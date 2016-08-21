using System.Collections.Generic;

namespace WebServices.ApiModel.Mappers.Contracts
{
    public interface IApiModelGraphMapper
    {
        Domain.Model.Graph Map(Graph graph);

        Graph Map(Domain.Model.Graph graph);
    }
}