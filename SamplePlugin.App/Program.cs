using Autofac;
using Autofac.Extras.CommonServiceLocator;
using bbv.Common.EventBroker;
using Microsoft.Owin.Hosting;
using Microsoft.Practices.ServiceLocation;
using SamplePlugin.App.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SamplePlugin.App
{
    public class Program
    {
        static void Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetName().Name + ".Hello.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                Console.WriteLine(result);
                Console.WriteLine();
            }

            // search for assemblies in the plugin folder
            PluginUtils.SetupPluginAssemblyPath();

            // list all assemblies in the plugins folder
            IList<Assembly> allAssemblies = PluginUtils.FindPluginAssemblies();

            // auto-load all modules
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(allAssemblies.ToArray());

            // register the Event Broker
            var eventBroker = new EventBroker();
            builder.RegisterInstance(eventBroker).As<IEventBroker>();

            // register the Nancy modules
            builder.RegisterType<HomeModule>();

            using (var container = builder.Build())
            {
                // setup serviceLocator
                var csl = new AutofacServiceLocator(container);
                ServiceLocator.SetLocatorProvider(() => csl);

                var url = "http://+:9898";
                using (WebApp.Start<Startup>(url))
                {
                    Console.WriteLine("Running on {0}", url);
                    Console.WriteLine("Press enter to exit");
                    Console.ReadKey(true);
                }
            }
        }
    }
}
