using Microsoft.Practices.ServiceLocation;
using Nancy;
using SamplePlugin.Api;
using SamplePlugin.App.ViewModel;
using System.Linq;

namespace SamplePlugin.App.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = parameters =>
            {
                // service locator
                var services = ServiceLocator.Current.GetAllInstances<IService>();

                var serviceItems = services.Select(x => new ServiceItem { Body = x.Body, Title = x.Title }).ToArray();

                return View["index", new HomeModel { Services = serviceItems }];
            };
        }
    }
}
