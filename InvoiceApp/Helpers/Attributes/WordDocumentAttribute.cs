using System.Web.Mvc;

namespace InvoiceApp.Helpers.Attributes
{
    /// <summary>
    /// Its mainly focused on the file manipulations. render views as Word documents.
    /// </summary>
    
    public class WordDocumentAttribute : ActionFilterAttribute
    {
        public string DefaultFilename { get; set; }

        /// <summary>
        /// Called after the action method is invoked.
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var result = filterContext.Result as ViewResult;
            if (result != null)
                result.MasterName = "_LayoutWord";
            filterContext.Controller.ViewBag.WordDocumentMode = true;
            base.OnActionExecuted(filterContext);
        }
        
        /// <summary>
        /// method calling after the action result executes
        /// </summary>
        /// <param name="filterContext"></param>
         
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var filename = filterContext.Controller.ViewBag.WordDocumentFilename;
            filename = filename ?? DefaultFilename ?? "Document";

          //  var test = filterContext.RequestContext.HttpContext.Request.ContentLength.ToString();


           // filterContext.HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            filterContext.HttpContext.Response.AppendHeader("content-disposition", string.Format("filename={0}.doc", filename));
          //  filterContext.HttpContext.Response.AppendHeader("content-length");
             filterContext.HttpContext.Response.ContentType = "application/msword";

            filterContext.HttpContext.Response.End();
            base.OnResultExecuted(filterContext);
        }
    }
}