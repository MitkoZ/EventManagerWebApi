using System.Web.Mvc;

namespace EventManager.Helpers
{
    public class ErrorHandlerAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.Result = new ViewResult { ViewName = "ServerError" };
            (filterContext.Result as ViewResult).ViewBag.UserMadeError = filterContext.Exception.Message; //this has to be put into a log file
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 500;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}