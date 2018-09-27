using System.Web;

namespace InvoiceApp.Helpers.Security
{
    public interface ISessionManager
    {
        string UserId { get; set; }

        string UserName { get; set; }

        int UploadedId { get; set; }

        ISessionManager Current { get; set; }

        string RoleName { get; set; }

        int EmployeeId { get; set; }

        int CompanyEmployeeId { get; set; }

        bool ishandover { get; set; }
        bool isTakeover { get; set; }

        string FirstName { get; set; }
        string LastName { get; set; }
        string UserGuid { get; set; }
        int selectedType { get; set; }
        int? EmpCode { get; set; }
        string EmailId { get; set; }
        string UploadedLmsTutorialPath { get; set; }
        string UploadedNewsUpdtesPath { get; set; }
        string UploadedLeavePoliciesPath { get; set; }
        int LeaveRequestid { get; set; }

        int CompanyId { get; set; }

        string LMSUserName { get; set; }
    }

    public class SessionManager : ISessionManager
    {
        /// <summary>
        /// it used to get current session values 
        /// </summary>
        public ISessionManager Current
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    lock (HttpContext.Current)
                    {
                        //Creating an object for the session manager
                        SessionManager session = (SessionManager)HttpContext.Current.Session["__MySession__"];

                        //Checking if the session is null
                        while (session == null)
                        {
                            Current = new SessionManager();
                            session = (SessionManager)HttpContext.Current.Session["__MySession__"];
                        }
                        //Returns the session
                        return session;
                    }
                }

                return new SessionManager();

            }
            set { HttpContext.Current.Session["__MySession__"] = value; }
        }

        //private static bool _enableDataUploads = UploadFileBusinessLogic.GetDataUploadListStatus();

        public string RefDelimiter { get; set; }
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string NotificationId { get; set; }

        public int UploadedId { get; set; }

        public int? EmpCode { get; set; }
        public string EmailId { get; set; }
        public int EmployeeId { get; set; }
        public int CompanyId { get; set; }

        public int LeaveRequestid { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserGuid { get; set; }

        public string RoleName { get; set; }

        public string UploadedLmsTutorialPath { get; set; }

        public string UploadedNewsUpdtesPath { get; set; }

        public string UploadedLeavePoliciesPath { get; set; }
        public int selectedType { get; set; }

        public string LMSUserName { get; set; }

        public bool ishandover { get; set; }

        public bool isTakeover { get; set; }

        //mock settings for testing
        private static SessionManager _mSession;
        /// <summary>
        /// used to manage the mock session testing 
        /// </summary>
        public static void SetMockSessionManager()
        {
            _mSession = new SessionManager();
        }




        public int CompanyEmployeeId { get; set; }
    }
}