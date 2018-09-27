using System.Web.Mvc;

namespace InvoiceApp.Areas.Master
{
    public class MasterAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Master";
            }
        }


        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Master_default",
                "Master/{controller}/{action}/{id}",
                new { controller = "Master", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}