using Autofac;
using SamplePlugin.Api;

namespace SamplePlugin.Payment
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PaymentService>().As<IService>().SingleInstance();
        }
    }
}
