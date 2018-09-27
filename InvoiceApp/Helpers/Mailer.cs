using System;
using System.Net.Mail;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web.Mvc;


namespace InvoiceApp.Helpers
{
    public interface IMailer
    {
        /// <summary>
        /// Config Mail
        /// </summary>
        void Configure();

        /// <summary>
        /// Send Mail
        /// </summary>
        void SendMail(MailMessage mail);
        void SendLMSMail(MailMessage mail);

        string GetEmailBodyFromTemplate(ControllerContext context,
            string viewPath,
            object model = null);
    }

    /// <summary>
    /// Mailer class for configuring and sending notification mail
    /// </summary>
    public class Mailer : IMailer, IDisposable
    {
        /// <summary>
        /// Mail Client
        /// </summary>
        SmtpClient smtp=new SmtpClient();

        //private ILogger logger;

        // Dispose() calls Dispose(true)
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        

        // NOTE: Leave out the finalizer altogether if this class doesn't 
        // own unmanaged resources itself, but leave the other methods
        // exactly as they are. 
        ~Mailer()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }
        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
           
        }

        /// <summary>
        /// Configure MailObject
        /// </summary>
        /// <param name="smtp"></param>
        /// <returns></returns>
        public void Configure()
        {
            //logger.Debug("Mailer.Configer ENTERED");
            
            //Creating an instance for the smtp client
            smtp = new SmtpClient();

            //Adding the host,port no,credentials to the smtp object

            var smtpuid = ConfigurationManager.AppSettings["SMTP.UID"];
            var smtppwd = ConfigurationManager.AppSettings["SMTP.Password"];
            var port = ConfigurationManager.AppSettings["SMTP.Port"];
            var enableSsl = ConfigurationManager.AppSettings["SMTP.EnableSSL"];
            // string Domain = ConfigurationManager.AppSettings["SMTP.Domain"];
            //logger.Debug("SMTPUID = " + SMTPUID + " SMTPPWD = " + SMTPPWD + " port = " + port + " EnableSSL = " + EnableSSL);
            smtp.Host = ConfigurationManager.AppSettings["SMTP.Server"];

            if (!string.IsNullOrEmpty(smtppwd))
            {
                smtp.Credentials = new System.Net.NetworkCredential(smtpuid, smtppwd);
                // smtp.UseDefaultCredentials = true;
            }

            //  if (!string.IsNullOrEmpty(SMTPPWD))
            // SmtpServer.Credentials = new System.Net.NetworkCredential(SMTPUID, SMTPPWD);

            if (!string.IsNullOrEmpty(port))
                smtp.Port = Convert.ToInt32(port);

            if (!string.IsNullOrEmpty(enableSsl))
                smtp.EnableSsl = Convert.ToBoolean(enableSsl);
        }

        public void SendLMSMail(MailMessage mail)
        {
            //logger.Debug("Mailer.SendMail ENTERED");

            string result;
            try
            {
                result = ConfigurationManager.AppSettings["isLocalDevEnv"] ?? "false";
                //logger.Debug("Mailer.SendMail result = " + result);
            }
            catch (ConfigurationErrorsException)
            {
                result = "false";
            }
            if (result == "false")
            {
                Configure();
                //Adding the from address to the mail object
                mail.From = new MailAddress(ConfigurationManager.AppSettings["SMTP.FromAddress"]);
                //logger.Debug("SMTP.FromAddress = " + mail.From);

                mail.BodyEncoding = Encoding.UTF8;
                //Making the html body as true
                mail.IsBodyHtml = true;
                //Sending the mail object
                //logger.Debug("Sending the mail object...");
                smtp.Send(mail);
            }
        }

        /// <summary>
        /// To send Notification Mail to Borrower
        /// </summary>
        /// <param name="mail"></param>
        public void SendMail(MailMessage mail)
        {
            //logger.Debug("Mailer.SendMail ENTERED");

            string result;
            try
            {
                result = ConfigurationManager.AppSettings["isLocalDevEnv"] ?? "false";
                //logger.Debug("Mailer.SendMail result = " + result);
            }
            catch (ConfigurationErrorsException)
            { 
                result = "false"; 
            }
            if (result == "false")
            {
                Configure();

                string body = mail.Body.ToString();

                var alternateView = AlternateView.CreateAlternateViewFromString(body, null, System.Net.Mime.MediaTypeNames.Text.Html);

                #region Header logo from project file

                var headerfile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Images\\Mail_Images\\HRMS_Header.png");
                var logoCompanyHeader = new LinkedResource(headerfile);
                logoCompanyHeader.ContentId = "companyHeader";
                alternateView.LinkedResources.Add(logoCompanyHeader);

                #endregion

                #region Footer logo from project file

                var footerfile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Images\\Mail_Images\\HRMS_Footer.png");
                var logoCompanyFooter = new LinkedResource(footerfile);
                logoCompanyFooter.ContentId = "companyFooter";
                alternateView.LinkedResources.Add(logoCompanyFooter);

                #endregion

                mail.AlternateViews.Add(alternateView);
                mail.Body = body;

                mail.Attachments.Clear(); //Clearing all the unwanted attachments

                //Adding the from address to the mail object
                mail.From = new MailAddress(ConfigurationManager.AppSettings["SMTP.FromAddress"]);
                //logger.Debug("SMTP.FromAddress = " + mail.From);

                mail.BodyEncoding = Encoding.UTF8;
                //Making the html body as true
                mail.IsBodyHtml = true;
                //Sending the mail object
                //logger.Debug("Sending the mail object...");

                smtp.Send(mail);
            }
        }
        /// <summary>
        /// Get Email format from template
        /// </summary>
        /// <param name="context"></param>
        /// <param name="viewPath"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public string GetEmailBodyFromTemplate(ControllerContext context,
                                   string viewPath,
                                   object model = null)
        {
            // first find the ViewEngine for this view
             var viewEngineResult =  ViewEngines.Engines.FindPartialView(context, viewPath);

            if (viewEngineResult == null)
                throw new FileNotFoundException("View cannot be found.");

            // get the view and attach the model to view data
            var view = viewEngineResult.View;
            context.Controller.ViewData.Model = model;

            string result = null;

            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(context, view,
                                            context.Controller.ViewData,
                                            context.Controller.TempData,
                                            sw);
                view.Render(ctx, sw);
                result = sw.ToString();
            }

            return result;
        }
    }
}
        
     