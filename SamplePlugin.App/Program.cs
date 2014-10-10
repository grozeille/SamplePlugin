using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Microsoft.Practices.ServiceLocation;
using SamplePlugin.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SamplePlugin.App
{
    public class Program
    {
        private IMyService _service;

        static void Main(string[] args)
        {
            // list all assemblies in the plugins folder
            List<Assembly> allAssemblies = new List<Assembly>();
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "plugins");

            foreach (string dll in Directory.GetFiles(path, "*Module.dll"))
            {
                allAssemblies.Add(Assembly.LoadFile(dll));
            }

            // auto-load all modules
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(allAssemblies.ToArray());

            // run the main program
            builder.RegisterType<Program>();
            using (var container = builder.Build()) 
            {
                // Set the service locator to an AutofacServiceLocator
                var csl = new AutofacServiceLocator(container);
                ServiceLocator.SetLocatorProvider(() => csl);

                container.Resolve<Program>().Run();
            }

            Console.Write("Press a key to exit");
            Console.ReadKey();
            
            //AutoMock automock = AutoMock.GetLoose();
        }
        
        public Program(IMyService service)
        {
            this._service = service;
        }

        public void Run()
        {
            // dependency injection
            _service.SayHello();

            // service locator
            ServiceLocator.Current.GetInstance<IMyService>().SayHello();
        }
    }
}
