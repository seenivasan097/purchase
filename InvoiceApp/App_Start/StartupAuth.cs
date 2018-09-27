using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using InvoiceApp.Helpers;

namespace InvoiceApp
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            int expireTimeSpan;
            int.TryParse(ConfigurationManager.AppSettings["ExpireTimeSpan"], out expireTimeSpan);

            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                //LoginPath = new PathString("/UserManagement/User/Login"),
                LoginPath = new PathString("/Home/Home"),
                ExpireTimeSpan = TimeSpan.FromMinutes(expireTimeSpan),
                SlidingExpiration = true,
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = context =>
                    {
                        DateTimeOffset now = DateTimeOffset.UtcNow;
                        // Add ExpiresUtc to the OwinContext request object - we will be checking 
                        // for this guy in the AdminController/RemainingTime action method.
                        if (context.Properties.ExpiresUtc != null)
                        {   context.OwinContext.Request.Set("time.Remaining",
                                context.Properties.ExpiresUtc.Value.Subtract(now).TotalSeconds);
                        }
                        return Task.FromResult<object>(null);
                    }
                }
            });

            app.Use(typeof(AuthenticationMiddleware));

            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication();
        }
    }
}
