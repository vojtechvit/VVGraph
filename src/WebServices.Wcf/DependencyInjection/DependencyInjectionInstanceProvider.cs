using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace WebServices.Wcf.DependencyInjection
{
    public class DependencyInjectionInstanceProvider : IInstanceProvider
    {
        private readonly Type serviceType;

        public DependencyInjectionInstanceProvider(Type serviceType)
        {
            this.serviceType = serviceType;
        }

        public object GetInstance(InstanceContext instanceContext)
            => GetInstance(instanceContext, null);

        public object GetInstance(InstanceContext instanceContext, Message message)
            => ServiceProvider.Instance.GetService(serviceType);

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        { }
    }
}