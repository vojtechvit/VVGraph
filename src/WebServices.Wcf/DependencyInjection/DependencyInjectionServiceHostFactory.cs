using System;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace WebServices.Wcf.DependencyInjection
{
    public class DependencyInjectionServiceHostFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
            => new DependencyInjectionServiceHost(serviceType, baseAddresses);
    }
}