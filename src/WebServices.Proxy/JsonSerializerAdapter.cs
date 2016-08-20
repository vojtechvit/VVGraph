using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using WebServices.Proxy.Contracts;

namespace WebServices.Proxy
{
    public sealed class JsonSerializerAdapter : JsonSerializer, IJsonSerializer
    {
        public JsonSerializerAdapter()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        public string Serialize(object value)
        {
            using (var textWriter = new StringWriter())
            {
                Serialize(textWriter, value);

                return textWriter.ToString();
            }
        }
    }
}