using Autofac;
using SamplePlugin.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplePlugin.CatalogModule
{
    public class MyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(new MyService()).As<IMyService>();
        }
    }
}
