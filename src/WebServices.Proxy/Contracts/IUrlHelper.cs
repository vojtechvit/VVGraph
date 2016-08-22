using System;

namespace WebServices.Proxy.Contracts
{
    public interface IUrlHelper
    {
        Uri GetGraphUrl(Uri baseUrl, string graphName);
    }
}