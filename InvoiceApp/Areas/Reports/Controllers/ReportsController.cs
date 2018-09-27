using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using InvoiceApp.App_Start;
using InvoiceApp.Controllers;
using InvoiceApp.Helpers;
using InvoiceApp.Helpers.Mappers;

namespace InvoiceApp.Areas.Candidates.Controllers
{
    [CustomAuthorize()]
    public partial class ReportsController : ApplicationBaseController
    {
        #region "Private variables"

        //Variable defines to Access the methods which is created in ILogger.cs file 
        private readonly ILogger _logger;

     

        #endregion "Private variables"

        /// <summary>
        /// Constructor Method
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="applicationClient"></param>
        /// <param name="applicationMapper"></param>
        public ReportsController(ILogger logger)
        {
            _logger = logger;
        }


       
    }
}