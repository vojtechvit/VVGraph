namespace WebServices.Proxy.Contracts
{
    public interface IVVGraphClientFactory
    {
        IVVGraphClient Create(VVGraphClientConfiguration configuration);
    }
}