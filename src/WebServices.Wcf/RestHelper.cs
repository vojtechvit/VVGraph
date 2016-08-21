using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using WebServices.Wcf.Contracts;

namespace WebServices.Wcf
{
    public sealed class RestHelper : IRestHelper
    {
        private readonly JsonSerializer jsonSerializer;

        public RestHelper()
        {
            jsonSerializer = new JsonSerializer
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public Message Ok(object value)
        {
            using (var stringWriter = new StringWriter())
            {
                jsonSerializer.Serialize(stringWriter, value);
                return WebOperationContext.Current.CreateTextResponse(stringWriter.ToString(), "application/json");
            }
        }

        public Message NoContent()
        {
            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NoContent;
            WebOperationContext.Current.OutgoingResponse.SuppressEntityBody = true;
            return WebOperationContext.Current.CreateTextResponse("");
        }

        public Message BadRequest()
        {
            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
            WebOperationContext.Current.OutgoingResponse.SuppressEntityBody = true;
            return WebOperationContext.Current.CreateTextResponse("");
        }

        public Message NotFound()
        {
            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
            WebOperationContext.Current.OutgoingResponse.SuppressEntityBody = true;
            return WebOperationContext.Current.CreateTextResponse("");
        }
    }
}