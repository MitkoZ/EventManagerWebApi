using EventManager.Helpers;
using System.Web.Mvc;
using System.Web.Routing;

namespace EventManager
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalFilters.Filters.Add(new Helpers.AuthorizeAttribute());
            GlobalFilters.Filters.Add(new ErrorHandlerAttribute());
        }
    }
}
