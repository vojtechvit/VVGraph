using System;
using System.ServiceModel;

namespace WebServices.Wcf.DependencyInjection
{
    public class DependencyInjectionServiceHost : ServiceHost
    {
        public DependencyInjectionServiceHost()
        {
        }

        public DependencyInjectionServiceHost(Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
        }

        protected override void OnOpening()
        {
            Description.Behaviors.Add(new DependencyInjectionServiceBehavior());
            base.OnOpening();
        }
    }
}