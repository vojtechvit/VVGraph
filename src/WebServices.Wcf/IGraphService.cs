using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using WebServices.ApiModel;

namespace WebServices.Wcf
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IGraphService
    {
        // GET api/v1/graphs/{graphName}
        [OperationContract]
        [WebGet(UriTemplate = "graphs/{graphName}", ResponseFormat = WebMessageFormat.Json)]
        Task<Message> GetAsync(string graphName);

        // PUT api/v1/graphs/{graphName}
        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "graphs/{graphName}/shortest-path", RequestFormat = WebMessageFormat.Json)]
        Task<Message> PutAsync(string graphName, Graph graph);

        // GET api/v1/graphs/{graphName}/shortest-path?startNodeId={startNodeId}&endNodeId={endNodeId}
        [OperationContract]
        [WebGet(UriTemplate = "graphs/{graphName}/shortest-path?startNodeId={startNodeId}&endNodeId={endNodeId}", ResponseFormat = WebMessageFormat.Json)]
        Task<Message> ShortestPathAsync(string graphName, int startNodeId, int endNodeId);
    }
}