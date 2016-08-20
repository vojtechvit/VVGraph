using Newtonsoft.Json;
using System.IO;
using WebServices.Proxy.Contracts;

namespace WebServices.Proxy
{
    public sealed class JsonSerializerAdapter : JsonSerializer, IJsonSerializer
    {
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