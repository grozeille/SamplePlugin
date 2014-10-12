using Autofac;
using SamplePlugin.Api;
using SamplePlugin.Catalog.Api;
using SamplePlugin.Catalog.Api.Services;
using SamplePlugin.Catalog.Services;

namespace SamplePlugin.Catalog
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CatalogService>().As<IService>().As<ICatalogService>().SingleInstance();
            builder.RegisterInstance(new CatalogRepository()).As<ICatalogRepository>().SingleInstance();
        }
    }
}
