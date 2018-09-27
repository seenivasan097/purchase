using System.Web;
using System.Web.Mvc;
using log4net;
using System.ServiceModel;

namespace InvoiceApp.Helpers
{
    /// <summary>
    /// Custom error handler to handle exceptions 
    /// </summary>
    public class CustomHandleErrorAttribute : HandleErrorAttribute
    {
        private readonly ILog _logger;
        /// <summary>
        /// to handling the custom error attribute
        /// </summary>
        public CustomHandleErrorAttribute()
        {
            _logger = LogManager.GetLogger(typeof(Log4NetWrapper));
        }
        /// <summary>
        /// to handling the error Exception details and displaying in UI 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }

            if (new HttpException(null, filterContext.Exception).GetHttpCode() != 500)
            {
                return;
            }

            if (!ExceptionType.IsInstanceOfType(filterContext.Exception))
            {
                return;
            }

            //Skip checked exception
            //if (filterContext.Exception is FaultException<ValidationFault>)
            //{
            //    return;
            //}

            // if the request is AJAX return JSON else view.
            if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new
                    {
                        error = true,
                        message = filterContext.Exception.Message
                    }
                };
            }
            else
            {
                var controllerName = (string)filterContext.RouteData.Values["controller"];
                var actionName = (string)filterContext.RouteData.Values["action"];
                var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);

                filterContext.Result = new ViewResult
                {
                    ViewName = View,
                    MasterName = Master,
                    ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                    TempData = filterContext.Controller.TempData
                };
            }

            // log the error using log4net.
            _logger.Error(filterContext.Exception.Message, filterContext.Exception);

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 500;

            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }

    }
}