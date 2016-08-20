using System;
using System.Runtime.Serialization;

namespace WebServices.AspNetCore.Proxy
{
    [Serializable]
    public sealed class VVGraphClientException : Exception
    {
        public VVGraphClientException()
        {
        }

        public VVGraphClientException(string message) : base(message)
        {
        }

        public VVGraphClientException(string message, Exception innerException) : base(message, innerException)
        {
        }

        private VVGraphClientException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}