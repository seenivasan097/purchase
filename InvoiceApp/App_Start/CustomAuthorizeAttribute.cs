using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using InvoiceApp.Helpers.Security;

namespace InvoiceApp.App_Start
{
    /// <summary>
    /// Implementing Custom authorisation based on web.config Authorization
    /// </summary> 
    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public CustomAuthorizeAttribute()
        {
            SetsessionManager(new SessionManager());
            NewAuthorizationHelper(new AuthorizationHelper());
        }

        public ISessionManager SessionManager { get; set; }

        public IAuthorizationHelper _authorizationHelper { get; set; }

        private void SetsessionManager(ISessionManager sessionManager)
        {
            SessionManager = sessionManager;
        }

        private void NewAuthorizationHelper(IAuthorizationHelper authorizationHelper)
        {
            _authorizationHelper = authorizationHelper;
        }


        // Custom property
        public string AccessLevel { get; set; }
        /// <summary>
        /// Returns a boolean value
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (string.IsNullOrEmpty(SessionManager.Current.UserId))
            {
                return false;
            }
            else
            {
                return true;

            }
           
        }


        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var controllerName = filterContext.RouteData.GetRequiredString("controller");
            var actionName = filterContext.RouteData.GetRequiredString("action");
            var verb = filterContext.HttpContext.Request.HttpMethod;

            if (!filterContext.RequestContext.HttpContext.Request.IsAjaxRequest() && !_authorizationHelper.ValidateUserRole(SessionManager.Current.RoleName, controllerName, actionName))
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Home", action = "UnAuthorisedError", area = "" })
                    );
            }
            if (SessionManager.Current.RoleName == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Home", action = "SessionExpired", area = "" })
                    );
            }

            base.OnAuthorization(filterContext);
        }
        /// <summary>
        /// sets the admin name and moves to the login controller if credentials are correct
        /// </summary>
        /// <param name="filterContext">AuthorizationContext</param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //if request is ajax request throw not authenticated status code
            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = 401;
                filterContext.HttpContext.Response.End();
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Home", action = "SessionExpired", area = "" })
                    );
            }
        }
    }
}