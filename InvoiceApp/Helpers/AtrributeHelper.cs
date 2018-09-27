using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace InvoiceApp.Helpers
{
    /// <summary>
    ///  Adding aria-required attribute for required fieldsH
    /// </summary>
    public class AtrributeHelper
    {

       /// <summary>
        /// "Get aria-required attribute for required fields
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="member"></param>
       /// <param name="isRequired"></param>
       /// <returns></returns>
        public static T GetAttribute<T>(MemberInfo member, bool isRequired) where T : Attribute
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
        /// <summary>
        /// Get attribute Display name property for required fields
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public static string GetPropertyDisplayName<T>(Expression<Func<T, object>> propertyExpression)
        {
            var memberInfo = GetPropertyInformation(propertyExpression.Body);
            if (memberInfo == null)
            {
                throw new ArgumentException(
                    "No property reference expression was found.",
                    "propertyExpression");
            }

            var attr = memberInfo.GetCustomAttribute<DisplayAttribute>(false);
            if (attr == null)
            {
                return memberInfo.Name;
            }

            return attr.Name;
        }
        /// <summary>
        /// Get attribute property Information for required fields
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
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
    }
}