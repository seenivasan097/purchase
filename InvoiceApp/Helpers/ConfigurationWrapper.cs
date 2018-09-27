using System.Configuration;
namespace InvoiceApp.Helpers
{
    public interface IConfiguration
    {
        string GetAppSettings(string name);
    }
    /// <summary>
    /// to read custom configuration entries (key/value) from appSettings 
    /// </summary>
    public class ConfigurationWrapper : IConfiguration
    {
        /// <summary>
        ///  to get values from appSetting values
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetAppSettings(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }
    }
}