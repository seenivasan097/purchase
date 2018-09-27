using System.Linq;

namespace InvoiceApp.Helpers.Security
{

    public interface IAuthorizationHelper
    {
        bool ValidateUserRole(string role, string controller, string action);
    }

    public class AuthorizationHelper : IAuthorizationHelper
    {

        public AuthorizationHelper()
        {
            SetsessionManager(new SessionManager());
        }

        private void SetsessionManager(ISessionManager sessionManager)
        {
            SessionManager = sessionManager;
        }

        public ISessionManager SessionManager { get; set; }

        public bool ValidateUserRole(string role, string controller, string action)
        {
            bool isAuthorized=false;
            if (role != null)
            {
                var rolesList = role.Split(',');
               
                switch (controller)
                {
                    case "User":
                        isAuthorized = rolesList.Contains("User");
                        break;
                    case "Admin":
                        isAuthorized = rolesList.Contains("Admin");
                        break;
                    case "Administrator":
                        isAuthorized = rolesList.Contains("Administrator");
                        break;
                    case "Supervisor":
                        isAuthorized = rolesList.Contains("Supervisor");
                        break;
                    case "Employee":
                        isAuthorized = rolesList.Contains("Employee");
                        break;
                    case "Configuration":
                        isAuthorized = rolesList.Contains("IsSuperAdmin");
                        if (!isAuthorized)
                        {
                            isAuthorized = rolesList.Contains("Configuration");
                        }
                        break;
                    case "JoinedEmployee":
                        isAuthorized = rolesList.Contains("IsSuperAdmin");
                        if (!isAuthorized)
                        {
                            isAuthorized = rolesList.Contains("Joined Employee Information");
                        }
                        if (!isAuthorized)
                        {
                            isAuthorized = rolesList.Contains("View Employee Information");
                        }
                        break;
                    case "Candidates":
                        isAuthorized = rolesList.Contains("IsSuperAdmin");
                        if (!isAuthorized)
                        {
                            isAuthorized = rolesList.Contains("Offered Candidate Information");
                        }
                        break;
                    default:
                        isAuthorized = false;
                        break;
                }
            }
            return isAuthorized;
        }
    }
}