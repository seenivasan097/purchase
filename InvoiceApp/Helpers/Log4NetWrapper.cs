using System;
using log4net;
using log4net.Config;

namespace InvoiceApp.Helpers
{
    public interface ILogger
    {
        void Info(string message);
        void Info(string message, Exception ex);
        void Client(string message);
        void Client(string message, Exception ex);
        void Notification(string message);
        void Notification(string message, Exception ex);
        void Warn(string message);
        void Warn(string message, Exception ex);
        void Error(string message);
        void Error(string message, Exception ex);
        void Debug(string message);
        void Debug(string message, Exception ex);

    }
    /// <summary>
    /// Used to manipulate the application logs in database
    /// </summary>
    public class Log4NetWrapper : ILogger
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(Log4NetWrapper));
        private readonly ILog _notification = LogManager.GetLogger("Notification");
        private readonly ILog _log = LogManager.GetLogger("Logging");
        private readonly ILog _client = LogManager.GetLogger("ClientException");
        private readonly ILog _debugLog = LogManager.GetLogger("DEBUG");
        private readonly ILog _warnLog = LogManager.GetLogger("WARN");
        private readonly ILog _infoLog = LogManager.GetLogger("INFO");
        private readonly ILog _errorLog = LogManager.GetLogger("ERROR");
        // private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static Log4NetWrapper()
        {
            XmlConfigurator.Configure();
        }
        /// <summary>
        /// Used to Enterd the  application logs in database based on the Log level
        /// </summary>
        /// <param name="message"></param>
        public void Debug(string message)
        {
            lock (_debugLog)
            {
                LogicalThreadContext.Properties["UserID"] = "Security Admin";
                LogicalThreadContext.Properties["CreatedBy"] = "System Exception";
                _debugLog.Debug(message);
            }
        }
        /// <summary>
        /// Used to Enterd the  application logs in database based on the Log level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public void Debug(string message, Exception ex)
        {
            lock (_debugLog)
            {
                LogicalThreadContext.Properties["UserID"] = "Security Admin";
                LogicalThreadContext.Properties["CreatedBy"] = "System Exception";
                _debugLog.Debug(message, ex);
            }
        }
        /// <summary>
        /// Used to Enterd the  application logs in database based on the Log level
        /// </summary>
        /// <param name="message"></param>
        public void Notification(string message)
        {
            lock (_notification)
            {
                LogicalThreadContext.Properties["UserID"] = "Security Admin";
                LogicalThreadContext.Properties["CreatedBy"] = "System Exception";
                _notification.Info(message);
            }
        }

        /// <summary>
        /// Used to Enterd the  application logs in database based on the Log level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public void Notification(string message, Exception ex)
        {
            lock (_notification)
            {
                LogicalThreadContext.Properties["UserID"] = "Security Admin";
                LogicalThreadContext.Properties["CreatedBy"] = "System Exception";
                _notification.Info(message, ex);
            }
        }

        /// <summary>
        /// Used to Enterd the  application logs in database based on the Log level
        /// </summary>
        /// <param name="message"></param>
        public void Client(string message)
        {
            lock (_client)
            {
                LogicalThreadContext.Properties["UserID"] = "Security Admin";
                LogicalThreadContext.Properties["CreatedBy"] = "System Exception";
                _client.Error(message);
            }
        }

        /// <summary>
        /// Used to Enterd the  application logs in database based on the Log level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public void Client(string message, Exception ex)
        {
            lock (_client)
            {
                LogicalThreadContext.Properties["UserID"] = "Security Admin";
                LogicalThreadContext.Properties["CreatedBy"] = "System Exception";
                _client.Error(message, ex);
            }
        }

        /// <summary>
        /// Used to Enterd the  application logs in database based on the Log level
        /// </summary>
        /// <param name="message"></param>
        public void Info(string message)
        {
            lock (_infoLog)
            {
                LogicalThreadContext.Properties["UserID"] = "Security Admin";
                LogicalThreadContext.Properties["CreatedBy"] = "System Exception";
                _infoLog.Info(message);
            }
        }

        /// <summary>
        /// Used to Enterd the  application logs in database based on the Log level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public void Info(string message, Exception ex)
        {
            lock (_infoLog)
            {
                LogicalThreadContext.Properties["UserID"] = "Security Admin";
                LogicalThreadContext.Properties["CreatedBy"] = "System Exception";
                _infoLog.Info(message, ex);
            }
        }

        /// <summary>
        /// Used to Enterd the  application logs in database based on the Log level
        /// </summary>
        /// <param name="message"></param>
        public void Warn(string message)
        {
            lock (_warnLog)
            {
                LogicalThreadContext.Properties["UserID"] = "Security Admin";
                LogicalThreadContext.Properties["CreatedBy"] = "System Exception";
                _warnLog.Warn(message);
            }
        }

        /// <summary>
        /// Used to Enterd the  application logs in database based on the Log level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public void Warn(string message, Exception ex)
        {
            lock (_warnLog)
            {
                LogicalThreadContext.Properties["UserID"] = "Security Admin";
                LogicalThreadContext.Properties["CreatedBy"] = "System Exception";
                _warnLog.Warn(message, ex);
            }
        }

        /// <summary>
        /// Used to Enterd the  application logs in database based on the Log level
        /// </summary>
        /// <param name="message"></param>
        public void Error(string message)
        {
            lock (_errorLog)
            {
                LogicalThreadContext.Properties["UserID"] = "Security Admin";
                LogicalThreadContext.Properties["CreatedBy"] = "System Exception";
                _notification.Error(message);
            }
        }

        /// <summary>
        /// Used to Enterd the  application logs in database based on the Log level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public void Error(string message, Exception ex)
        {
            lock (_errorLog)
            {
                log4net.LogicalThreadContext.Properties["UserID"] = "Security Admin";
                log4net.LogicalThreadContext.Properties["CreatedBy"] = "System Exception";
                _errorLog.Error(ex.GetType().FullName, ex);
            }
        }

    }
}