namespace WebServices.Proxy.Contracts
{
    public interface IJsonSerializer
    {
        string Serialize(object value);
    }
}