using System;
using System.Web.Mvc;
using InvoiceApp.Helpers.Security;

namespace InvoiceApp.Controllers
{
    [T4MVC(false)]
    [AllowAnonymous()]
    public abstract partial class ApplicationBaseController : Controller
    {

        protected ApplicationBaseController()
        {
            SetsessionManager(new SessionManager());
        }

        public ISessionManager SessionManager { get; set; }

        private void SetsessionManager(ISessionManager sessionManager)
        {
            SessionManager = sessionManager;
            ViewBag.SessionManager = SessionManager;
        }

       
        /// <summary>
        /// override default value of MaxJsonLength.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <param name="contentEncoding"></param>
        /// <param name="behavior"></param>
        /// <returns></returns>
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }
    }
}