using System.Web;
using System.Web.Mvc;

namespace InvoiceApp.Helpers.Attributes
{
    ///<summary>  
    /// Prevent the auth cookie from being reset for this action, allows you to
    /// have requests that do not reset the login timeout.  
    /// </summary> 
    public class DoNotResetAuthCookieAttribute : ActionFilterAttribute
    {
 
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            filterContext.HttpContext.GetOwinContext().Environment.Add("StripAspCookie", true);
        }
    }
}
