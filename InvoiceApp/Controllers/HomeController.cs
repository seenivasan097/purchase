using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.DirectoryServices.AccountManagement;
using System;
using System.Configuration;
using InvoiceApp.Helpers;
using InvoiceApp.Helpers.Extensions;
using InvoiceApp.Helpers.Mappers;

namespace InvoiceApp.Controllers
{
    [AllowAnonymous()]
    public partial class HomeController : ApplicationBaseController
    {

        private readonly ILogger _logger;

        public HomeController(ILogger logger)
        {
            _logger = logger;
        }

        [AllowAnonymous()]
        public virtual ActionResult Index()
        {
            SessionManager.Current.RoleName = "Admin";
            //  var moduleId=(from rs in resultAccessPermissionList.AccessPermissionList  )

            return View();
        }

        [AllowAnonymous()]
        public virtual ActionResult Dashboard()
        {
            return View();
        }

        public virtual ViewResult SessionExpired()
        {
            return View();
        }

        public virtual ViewResult SessionExpiredPopup()
        {
            return View();
        }

        public virtual ViewResult UnAuthorisedError()
        {
            return View();
        }

      














    }
}