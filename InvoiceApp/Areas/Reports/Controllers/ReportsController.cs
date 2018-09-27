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
using HRMS.UI;

namespace InvoiceApp.Areas.Reports.Controllers
{

    //[CustomAuthorize()]
    public partial class ReportsController : ApplicationBaseController
    {
        ApplicationEntities db;
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
            db = new ApplicationEntities();
        }

        public virtual ActionResult ItemStockList()
        {
            return View();
        }

        public virtual JsonResult getItemStoxkList()
        {
            var items = (from im in db.ViwItemStock
                         join x in db.Items on im.ItemId equals x.ItemId
                         where x.IsActive == 1 
                         select new
                         {
                             ItemId = x.ItemId,
                             ItemCode = x.ItemCode,
                             ItemName = x.ItemName,
                             Unit = x.Unit,
                             Quantity = im.Quantity
                         }).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }
    }
}