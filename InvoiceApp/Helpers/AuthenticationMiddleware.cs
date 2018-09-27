using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;

namespace InvoiceApp.Helpers
{
    /// <summary>
    ///  interacts properly with the authentication pipeline
    /// </summary>

    public class AuthenticationMiddleware : OwinMiddleware
    {
        const string _authenticationCookie = CookieAuthenticationDefaults.CookiePrefix + DefaultAuthenticationTypes.ApplicationCookie;

        public AuthenticationMiddleware(OwinMiddleware next) :
            base(next) { }
       /// <summary>
        ///  overriding a method in a base class library
       /// </summary>
       /// <param name="context"></param>
       /// <returns></returns>
        public override async Task Invoke(IOwinContext context)
        {
            var response = context.Response;
            response.OnSendingHeaders(state =>
            {
                var resp = (OwinResponse)state;

                if (resp.Environment.ContainsKey("StripAspCookie"))
                {
                    resp.Cookies.Delete(_authenticationCookie);
                }
            }, response);

            await Next.Invoke(context);
        }
    }
}