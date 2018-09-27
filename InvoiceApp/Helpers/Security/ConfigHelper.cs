
using System.Configuration;

namespace InvoiceApp.Helpers.Security
{
    /// <summary>
    /// Helper class to get the app setting information from config.
    /// </summary>
    public static class ConfigHelper 
    {
         
        /// <summary>
        /// AD Group Name
        /// </summary>
        public static string GroupName
        {
            get
            {
                return ConfigurationManager.AppSettings["AD.GroupName"];
            }
        }

        /// <summary>
        /// AD Domain Name
        /// </summary>
        public static string DomainName
        {
            get
            {
                return ConfigurationManager.AppSettings["AD.DomainName"];
            }
        }

        /// <summary>
        /// Home Page URL
        /// </summary>
        public static string HomePageURL
        {
            get
            {
                return ConfigurationManager.AppSettings["HomePage"];
            }
        }

        /// <summary>
        /// Home Page URL
        /// </summary>
        public static string PAHomePageURL
        {
            get
            {
                return ConfigurationManager.AppSettings["PAHomePage"];
            }
        }

        /// <summary>
        /// SessionTimeout
        /// </summary>
        public static string SessionTimeout
        {
            get
            {
                return ConfigurationManager.AppSettings["SessionTimeout"];
            }
        }

        public static string SessionTimeoutPopup
        {
            get
            {
                return ConfigurationManager.AppSettings["SessionTimeoutPopup"];
            }
        }

        /// <summary>
        /// Home Page URL
        /// </summary>
        public static string UnAuthorizedURL
        {
            get
            {
                return ConfigurationManager.AppSettings["UnAuthorizedURL"];
            }
        }

        /// <summary>
        /// Home Page URL
        /// </summary>
        public static string CheckLogin
        {
            get
            {
                return ConfigurationManager.AppSettings["CheckLogin"];
            }
        }

        /// <summary>
        /// Default UserID
        /// </summary>
        public static string DefaultUserID
        {
            get
            {
                return ConfigurationManager.AppSettings["DefaultUserID"];
            }
        }

        /// <summary>
        /// Default UserName
        /// </summary>
        public static string DefaultUserName
        {
            get
            {
                return ConfigurationManager.AppSettings["DefaultUserName"];
            }
        }

        /// <summary>
        /// Home Page URL
        /// </summary>
        public static string EmployeeURL
        {
            get
            {
                return ConfigurationManager.AppSettings["EmployeeUrl"];
            }
        }

        /// <summary>
        /// Home Page URL
        /// </summary>
        public static string SupervisorURL
        {
            get
            {
                return ConfigurationManager.AppSettings["SupervisorUrl"];
            }
        }

        /// <summary>
        /// Home Page URL
        /// </summary>
        public static string AdministratorURL
        {
            get
            {
                return ConfigurationManager.AppSettings["AdministratorUrl"];
            }
        }
        
    }
}
