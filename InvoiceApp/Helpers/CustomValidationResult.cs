using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceApp.Helpers
{
    /// <summary>
    /// To handling the validation list and error message   
    /// </summary>
    public class CustomValidationResult
    {
        public bool IsValid { get; set; }

        public List<Tuple<string, string>> ErrorList { get; set; }
        /// <summary>
        /// To used to manage the error list based on validation result   
        /// </summary>
        public CustomValidationResult()
        {
            IsValid = true;
            ErrorList = new List<Tuple<string, string>>();
        }
        /// <summary>
        /// To used to manage the error list based on validation result
        /// convert to string   
        /// </summary>
        public string ToString()
        {
            var sb = new StringBuilder();
            foreach (var error in ErrorList)
            {
                sb.AppendFormat("{0} {1}<br/>", error.Item1, error.Item2);
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}