using System.Web.Mvc;

namespace InvoiceApp.Controllers
{
    public partial class ErrorController : Controller
    {
        /// <summary>
        ///GET: Error based on the Index
        /// </summary>
        [AllowAnonymous]
        public virtual ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// GET: Error file not found issue
        /// </summary>
        [AllowAnonymous]
        public virtual ActionResult FileNotFound()
        {
            return View();
        }
    }
}