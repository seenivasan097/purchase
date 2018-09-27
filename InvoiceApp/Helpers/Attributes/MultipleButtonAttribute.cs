using System;
using System.Reflection;
using System.Web.Mvc;

namespace InvoiceApp.Helpers.Attributes
{
    ///<summary>  
    /// its allow multiple submit button action in a view 
    /// </summary> 
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MultipleButtonAttribute : ActionNameSelectorAttribute
    {
        public string MatchFormKey { get; set; }
        public string MatchFormValue { get; set; }
        /// <summary>
        /// its validated action name and controller name  
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="actionName"></param>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            return controllerContext.HttpContext.Request[MatchFormKey] != null &&
                controllerContext.HttpContext.Request[MatchFormKey] == MatchFormValue;
        }
    }
}