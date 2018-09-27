using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;

namespace InvoiceApp.Helpers.Extensions
{
           ///<summary> 
            ///  write a simple extension method for the built in html helper class. 
            /// this will enable us to use the same Html object to use our custom helper method.
            /// </summary> 
   
    public static class ExtensionsHelper
    {
        ///<summary> 
        /// this method is used to adding the model state error list, 
        /// </summary> 
        /// <param name="modelState"></param>
        /// <param name="errorsList"></param>
        public static void AddModelStateErrors(this ModelStateDictionary modelState, List<string> errorsList)
        {
            if (errorsList == null) 
                return;

            foreach (var error in errorsList)
            {
                modelState.AddModelError("", error);
            }
        }


        ///<summary> 
        /// Makes it easier to retrieve custom attributes of a given type from a reflected type.
        /// </summary> 
        /// <param name="member"></param>
        /// <param name="isRequired"></param>
         public static T GetAttribute<T>(this MemberInfo member, bool isRequired) where T : Attribute
         {
             var attribute = member.GetCustomAttributes(typeof(T), false).SingleOrDefault();

             if (attribute == null && isRequired)
             {
                 throw new ArgumentException(
                     string.Format(
                         CultureInfo.InvariantCulture,
                         "The {0} attribute must be defined on member {1}",
                         typeof(T).Name,
                         member.Name));
             }

             return (T)attribute;
         }
         ///<summary> 
         /// it used to  get the actual string value of the resource associated with the given class property for testing purposes.
         /// </summary> 
         /// <param name="member"></param>
         /// <param name="isRequired"></param>
         public static string GetPropertyDisplayAttribute<T>(Expression<Func<T, object>> propertyExpression)
         {
             var memberInfo = GetPropertyInformation(propertyExpression.Body);
             if (memberInfo == null)
             {
                 throw new ArgumentException(
                     "No property reference expression was found.",
                     "propertyExpression");
             }

             var attr = memberInfo.GetAttribute<DisplayAttribute>(false);
             if (attr == null)
             {
                 return memberInfo.Name;
             }

             return attr.Name;   
         }
         /// <summary> 
         /// This method used to collect attribute property for he given expression
         /// </summary> 
         /// <param name="<T> Expression"></param>
         /// <param name="propertyExpression"></param>
         public static bool GetPropertyRequiredAttributeIsFound<T>(Expression<Func<T, object>> propertyExpression)
         {
             var memberInfo = GetPropertyInformation(propertyExpression.Body);
             if (memberInfo == null)
             {
                 throw new ArgumentException(
                     "No property reference expression was found.",
                     "propertyExpression");
             }

             var attr = memberInfo.GetAttribute<RequiredAttribute>(false);
             if (attr != null)
             {
                 return (!attr.AllowEmptyStrings);
             }

             return false;
         }
         /// <summary> 
         /// This method used to collect information  for he given Expression class property
         /// </summary> 
         /// <param name="Expression"></param>
         public static MemberInfo GetPropertyInformation(Expression propertyExpression)
         {
             Debug.Assert(propertyExpression != null, "propertyExpression != null");
             MemberExpression memberExpr = propertyExpression as MemberExpression;
             if (memberExpr == null)
             {
                 UnaryExpression unaryExpr = propertyExpression as UnaryExpression;
                 if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
                 {
                     memberExpr = unaryExpr.Operand as MemberExpression;
                 }
             }

             if (memberExpr != null && memberExpr.Member.MemberType == MemberTypes.Property)
             {
                 return memberExpr.Member;
             }

             return null;
         }

        
         /// <summary>
         ///  Helpers to get error message
         /// </summary>
         /// <param name="result"></param>
         /// <param name="errorsList"></param>
         /// <returns></returns>
        private static  string FirstOrDefaultErrorMessage  (bool result, List<string> errorsList)
        {
            if (result) return null;
            if (errorsList != null && errorsList.Any())
                return errorsList.First();

            return CommonStaticHelpers.CErrorMessage;
        }
    }
}
