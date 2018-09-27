using System.Web.Mvc;
using InvoiceApp.Helpers;

namespace InvoiceApp
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomHandleErrorAttribute());
        }
    }
}
