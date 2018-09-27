using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using InvoiceApp.Helpers.Mappers;
using System.Web.Helpers;
using System.Security.Claims;
using Datatables.Mvc;

namespace InvoiceApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutoMapperWebConfiguration.Configure();
            @AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;

            #region Binders
            //DataTableModelBinder
            ModelBinders.Binders[typeof(DataTable)] = new DataTableModelBinder();
            #endregion
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            if (ex is HttpException && ((HttpException)ex).GetHttpCode() == 404)
            {
            }
        }
    }
}
