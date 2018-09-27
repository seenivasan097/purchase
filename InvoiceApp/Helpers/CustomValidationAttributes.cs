using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace InvoiceApp.Helpers
{      
    /// <summary>
    /// to handling the validation details  
    /// </summary>
    public class CustomValidationAttributes
    {
        public class EnsureMinimumElementsAttribute : ValidationAttribute
        {
            private readonly int _minElements;
            /// <summary>
            /// Ensurinig the minElement values 
            /// </summary>
            /// <param name="minElements"></param>
            public EnsureMinimumElementsAttribute(int minElements)
            {
                _minElements = minElements;
            }
            /// <summary>
            /// to Checking the given data limit is greater than or less with minimum element value
            /// </summary>
            public override bool IsValid(object value)
            {
                var list = value as IList;
                if (list != null)
                {
                    return list.Count >= _minElements;
                }
                return false;
            }
        }
    }
}
