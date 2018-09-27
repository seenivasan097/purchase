using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace InvoiceApp.Helpers.Enumeration
{
    ///<summary>  
    /// its used to Rendering the content into view
    /// </summary> 
    public static class Helper
    {
        /// <summary>
        /// its used to get value and key from the list element
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static SelectList GetSelectedItemList<T>() where T : struct
        {
            T t = default(T);
            string exceptionMessage = "Please make sure that T is of Enum Type";

            if (!t.GetType().IsEnum){
                throw new ArgumentNullException(exceptionMessage);
            }
            var nameList = t.GetType().GetEnumNames();
            int counter = 0;
            
            Dictionary<int, string> myDictionary= new Dictionary<int, string>();
            if (nameList != null && nameList.Length > 0)
            {
                foreach (var name in nameList)
                {
                    T newEnum = (T) Enum.Parse(t.GetType(), name);
                    string description = GetDescriptionFromEnumValue(newEnum as Enum);


                    var key = (int)Convert.ChangeType(newEnum, Enum.GetUnderlyingType(newEnum.GetType()));
                    if (!myDictionary.ContainsKey(key))
                    {
                        myDictionary.Add(key, description);
                    }
                    counter++;
                }
                counter = 0;
                return new SelectList(myDictionary,"Key","Value");
            }
            
            return null;
        }
        /// <summary>
        /// Getting enum variable descriptions
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
 
        public static string GetDescriptionFromEnumValue(Enum value)
        {
            DescriptionAttribute descriptionAttribute = 
                value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute),false)
                .SingleOrDefault() as DescriptionAttribute;
            return descriptionAttribute == null ? 
                value.ToString() : descriptionAttribute.Description;
        }
      
    }
}