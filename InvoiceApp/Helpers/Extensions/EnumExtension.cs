using System;
using InvoiceApp.Helpers.Attributes;

namespace InvoiceApp.Helpers.Extensions
{
    public static class EnumExtension
    {
        /// <summary>
        /// Will get the string value for a given enums value, this will
        /// only work if you assign the StringValue attribute to
        /// the items in your enum.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetStringValue(this Enum value)
        {
            ///<summary> 
            /// Get the type
            /// </summary> 
            var type = value.GetType();
            ///<summary> 
            /// Get fieldinfo for this type
            /// </summary> 
            var fieldInfo = type.GetField(value.ToString());
            ///<summary> 
            /// Get the stringvalue attributes
            /// </summary> 
            
            var attribs = fieldInfo.GetCustomAttributes(
                typeof(StringValueAttribute), false) as StringValueAttribute[];
            ///<summary> 
            /// Return the first if there was a match.
            /// </summary> 
           return attribs != null && attribs.Length > 0 ? attribs[0].StringValue : null;
        } 
    }
}
 