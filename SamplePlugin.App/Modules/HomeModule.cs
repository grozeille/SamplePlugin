using Nancy;

namespace SamplePlugin.App.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = parameters =>
            {
                return "Hello";
            };
        }
    }
}
