using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InvoiceApp.Controllers;
using InvoiceApp.Helpers;
using InvoiceApp.Helpers.Mappers;
using InvoiceApp.App_Start;
using System.Net.Mail;
using System.IO;
using HRMS.UI;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRMS.UI.Areas.Master.Models;
using HRMS.UI.Areas.Transaction.Models;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data;
using Microsoft.SqlServer.Server;

namespace InvoiceApp.Areas.Transaction.Controllers
{
    //[CustomAuthorize()]
    public partial class TransactionController : ApplicationBaseController
    {
        ApplicationEntities db;

        #region "Private variables"

        private readonly ILogger _logger;

        #endregion "Private variables"
        #region "Public variables"

        /// <summary>
        /// Constructor Method
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="applicationClient"></param>
        /// <param name="applicationMapper"></param>
        public TransactionController(ILogger logger)
        {
            _logger = logger;
            db = new ApplicationEntities();
        }
        #endregion "Public variables"

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        public virtual ActionResult Purchase()
        {
            return View();
        }

        public virtual ActionResult EditPurchase(int POId)
        {
            return View("~/Areas/Transaction/Views/Transaction/Purchase.cshtml");
        }

        public virtual ActionResult EditGRN(int GRNId)
        {
            return View("~/Areas/Transaction/Views/Transaction/GRN.cshtml");
        }

        public virtual ActionResult GRN()
        {
            return View();
        }
        public virtual ActionResult Invoice()
        {
            return View();
        }
        public virtual ActionResult OpeningStock()
        {
            return View();
        }

        public virtual ActionResult PurchaseOrderList()
        {
            return View();
        }
        public virtual ActionResult GRNList()
        {
            return View();
        }
        public virtual ActionResult InvoiceList()
        {
            return View();
        }

        public virtual JsonResult GetCurrentAccountYear()
        {
            string accountYear = db.getAccountYear().FirstOrDefault().FYEAR;
            return Json(accountYear, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult GetCurrentPONo()
        {
            int IncNo = 0;
            string PONo = "";
            GetPONo(out IncNo, out PONo);
            return Json(PONo, JsonRequestBehavior.AllowGet);
        }

        public string GetAccountYear()
        {
            string accountYear = db.getAccountYear().FirstOrDefault().FYEAR;
            return accountYear;
        }

        public void GetPONo(out int IncNo, out string PONo)
        {
            var accountYear = db.getAccountYear().FirstOrDefault().FYEAR;

            int IncrementId;
            int? maxid = db.PurchaseOrder.Where(x => x.AccountYear == accountYear).Max(u => (int?)u.IncrementId);
            if (maxid == null)
            {
                IncrementId = 1;
            }
            else
            {
                IncrementId = maxid.Value;
                IncrementId += 1;
            }
            PONo = "PO" + IncrementId.ToString("D4") + "/" + accountYear;
            IncNo = IncrementId;
        }

        public virtual JsonResult getItemListByCustomer(int CustomerId, string prefix)
        {
            var items = (from im in db.ItemMappings
                         join sm in db.SubItemMappings on im.ItemMappingId equals sm.ItemMappingId
                         join x in db.Items on sm.ItemId equals x.ItemId
                         where x.IsActive == 1 && sm.IsActive == 1 && im.CustomerId == CustomerId && x.ItemName.Contains(prefix)
                         select new ItemModel()
                         {
                             ItemId = x.ItemId,
                             ItemCode = x.ItemCode,
                             ItemName = x.ItemName,
                             Unit = x.Unit,
                             Rate = x.Rate
                         }).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult SavePurchaseOrder(SavePOModel model)
        {
            var result = false;
            try
            {
                model.POModel.POType = "";
                if (model.POModel.POId > 0)
                {
                    var poModel = db.PurchaseOrder.Where(x => x.POId == model.POModel.POId).FirstOrDefault();
                    poModel.POType = model.POModel.POType;
                    poModel.PODate = model.POModel.PODate;
                    poModel.IntentNo = model.POModel.IntentNo;
                    poModel.IntentDate = model.POModel.IntentDate;
                    poModel.EnquiryNo = model.POModel.EnquiryNo;
                    poModel.EnquiryDate = model.POModel.EnquiryDate;
                    poModel.QuotationNo = model.POModel.QuotationNo;
                    poModel.QuotationDate = model.POModel.QuotationDate;
                    poModel.SupplierId = model.POModel.SupplierId;
                    poModel.BillDate = model.POModel.BillDate;
                    poModel.RequestedBy = model.POModel.RequestedBy;
                    poModel.POType = model.POModel.POType;
                    poModel.DueDate = model.POModel.DueDate;
                    poModel.SubTotal = model.POModel.SubTotal;
                    poModel.CGST = model.POModel.CGST;
                    poModel.SGST = model.POModel.SGST;
                    poModel.IGST = model.POModel.IGST;
                    poModel.Total = model.POModel.Total;
                    poModel.DeliverySchedule = model.POModel.DeliverySchedule;
                    poModel.DeliveryType = model.POModel.DeliveryType;
                    poModel.Modifiedby = 1;
                    poModel.Modifiedon = DateTime.Now;
                    poModel.CourierCharges = model.POModel.CourierCharges;

                    var existSubItemList = db.SubPurchaseOrder.Where(x => x.POId == model.POModel.POId && x.IsActive == 1).ToList();
                    existSubItemList.ForEach(x => x.IsActive = 0);
                    existSubItemList.ForEach(x => x.Modifiedby = 1);
                    existSubItemList.ForEach(x => x.Modifiedon = DateTime.Now);
                    model.POSubItemList.ForEach(x => x.POId = model.POModel.POId);
                    model.POSubItemList.ForEach(x => x.Createdby = 1);
                    model.POSubItemList.ForEach(x => x.Createdon = DateTime.Now);
                    foreach (var objsubitem in model.POSubItemList)
                    {
                        db.SubPurchaseOrder.Add(objsubitem);
                    }

                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    int IncNo = 0;
                    string PONo = "";
                    GetPONo(out IncNo, out PONo);
                    model.POModel.AccountYear = GetAccountYear();
                    model.POModel.PONo = PONo;
                    model.POModel.IncrementId = IncNo;
                    model.POModel.IsActive = 1;
                    model.POModel.Createdby = 1;
                    model.POModel.Createdon = DateTime.Now;
                    db.PurchaseOrder.Add(model.POModel);
                    db.SaveChanges();
                    model.POSubItemList.ForEach(x => x.POId = model.POModel.POId);
                    model.POSubItemList.ForEach(x => x.Createdby = 1);
                    model.POSubItemList.ForEach(x => x.Createdon = DateTime.Now);
                    foreach (var objsubitem in model.POSubItemList)
                    {
                        db.SubPurchaseOrder.Add(objsubitem);
                    }
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error in mapping." }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, message = "Saved successfully." }, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult POList()
        {

            var ItemList = (from im in db.PurchaseOrder
                            join cus in db.Customers on im.SupplierId equals cus.CustomerId
                            where im.IsActive == 1 && cus.IsActive == 1
                            select new PurchaseOrderModel()
                            {
                                AccountYear = im.AccountYear,
                                BillDate = im.BillDate,
                                CGST = im.CGST,
                                DeliverySchedule = im.DeliverySchedule,
                                DeliveryType = im.DeliveryType,
                                DueDate = im.DueDate,
                                EnquiryDate = im.EnquiryDate,
                                EnquiryNo = im.EnquiryNo,
                                IGST = im.IGST,
                                IncrementId = im.IncrementId,
                                IntentDate = im.IntentDate,
                                IntentNo = im.IntentNo,
                                PODate = im.PODate,
                                POId = im.POId,
                                PONo = im.PONo,
                                POType = im.POType,
                                QuotationDate = im.QuotationDate,
                                QuotationNo = im.QuotationNo,
                                RequestedBy = im.RequestedBy,
                                SGST = im.SGST,
                                SubTotal = im.SubTotal,
                                SupplierId = im.SupplierId,
                                SupplierName = cus.CustomerName,
                                Total = im.Total
                            }).OrderByDescending(x => x.POId).ToList();

            return Json(ItemList, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult GetPOById(int POId)
        {

            var POModel = (from im in db.PurchaseOrder
                           join cus in db.Customers on im.SupplierId equals cus.CustomerId
                           where im.IsActive == 1 && cus.IsActive == 1 && im.POId == POId
                           select new PurchaseOrderModel()
                           {
                               AccountYear = im.AccountYear,
                               BillDate = im.BillDate,
                               CGST = im.CGST,
                               DeliverySchedule = im.DeliverySchedule,
                               DeliveryType = im.DeliveryType,
                               DueDate = im.DueDate,
                               EnquiryDate = im.EnquiryDate,
                               EnquiryNo = im.EnquiryNo,
                               IGST = im.IGST,
                               IncrementId = im.IncrementId,
                               IntentDate = im.IntentDate,
                               IntentNo = im.IntentNo,
                               PODate = im.PODate,
                               POId = im.POId,
                               PONo = im.PONo,
                               POType = im.POType,
                               QuotationDate = im.QuotationDate,
                               QuotationNo = im.QuotationNo,
                               RequestedBy = im.RequestedBy,
                               SGST = im.SGST,
                               SubTotal = im.SubTotal,
                               SupplierId = im.SupplierId,
                               SupplierName = cus.CustomerName,
                               Total = im.Total,
                               CourierCharges = im.CourierCharges
                           }).FirstOrDefault();

            var POItemModel = (from im in db.PurchaseOrder
                               join sub in db.SubPurchaseOrder on im.POId equals sub.POId
                               join item in db.Items on sub.ItemId equals item.ItemId
                               where im.IsActive == 1 && sub.IsActive == 1 && im.POId == POId
                               select new PurchaseOrderItemModel()
                               {
                                   SubPOId = sub.SubPOId,
                                   POId = sub.POId,
                                   ItemId = sub.ItemId,
                                   ItemName = item.ItemName,
                                   ItemUnit = item.Unit,
                                   Quantity = sub.Quantity,
                                   Rate = sub.Rate,
                                   CGSTPer = sub.CGSTPer,
                                   CGSTAmount = sub.CGSTAmount,
                                   SGSTPer = sub.SGSTPer,
                                   SGSTAmount = sub.SGSTAmount,
                                   IGSTPer = sub.IGSTPer,
                                   IGSTAmount = sub.IGSTAmount,
                                   Total = sub.Total
                               }).ToList();
            var data = new { PoModel = POModel, POItemModel = POItemModel };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual JsonResult DeletePO(int POId)
        {
            bool result = false;
            PurchaseOrder po = db.PurchaseOrder.Where(x => x.POId == POId).FirstOrDefault();
            if (po != null)
            {
                po.IsActive = 0;
                po.Modifiedon = DateTime.Now;
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult GetCurrentOPNo()
        {
            int IncNo = 0;
            string PONo = "";
            GetOPNo(out IncNo, out PONo);
            return Json(PONo, JsonRequestBehavior.AllowGet);
        }

        public void GetOPNo(out int IncrementId, out string OPNO)
        {
            var accountYear = db.getAccountYear().FirstOrDefault().FYEAR;

            ;
            int? maxid = db.OpeningStock.Where(x => x.AccountYear == accountYear).Max(u => (int?)u.IncrementId);
            if (maxid == null)
            {
                IncrementId = 1;
            }
            else
            {
                IncrementId = maxid.Value;
                IncrementId += 1;
            }
            OPNO = "OP" + IncrementId.ToString("D4") + "/" + accountYear;
        }

        [HttpPost]
        public virtual JsonResult SaveOpeningStock(OpeningStock model)
        {
            var result = false;
            try
            {

                if (model.OpeningStockId > 0)
                {
                    OpeningStock OpeningStock = db.OpeningStock.Where(x => x.OpeningStockId == model.OpeningStockId).FirstOrDefault();
                    OpeningStock.ItemId = model.ItemId;
                    OpeningStock.ActualStock = model.ActualStock;
                    OpeningStock.PhysicalStock = model.PhysicalStock;
                    OpeningStock.PreparedBy = model.PreparedBy;
                    OpeningStock.Remarks = model.Remarks;
                    OpeningStock.Modifiedby = 1;

                    OpeningStock.Modifiedon = DateTime.Now;
                    db.SaveChanges();

                    db.proc_ItemOpeningStock("Update", model.OpeningStockNo, OpeningStock.Modifiedon, model.ItemId, model.ActualStock);
                    result = true;
                }
                else
                {
                    int IncrementId = 0;
                    string OPNo = "";
                    GetOPNo(out IncrementId, out OPNo);
                    model.OpeningStockNo = OPNo;
                    model.IncrementId = IncrementId;
                    model.IsActive = 1;
                    model.Createdby = 1;
                    model.Createdon = DateTime.Now;
                    db.OpeningStock.Add(model);
                    db.SaveChanges();
                    db.proc_ItemOpeningStock("Add", model.OpeningStockNo, model.Createdon, model.ItemId, model.ActualStock);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error in save." }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, message = "Saved successfully." }, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult Getopeningstocklist()
        {

            var list = (from o in db.OpeningStock
                        join i in db.Items on o.ItemId equals i.ItemId
                        where o.IsActive == 1
                        select new
                        {
                            OpeningStockId = o.OpeningStockId,
                            AccountYear = o.AccountYear,
                            IncrementId = o.IncrementId,
                            OpeningStockNo = o.OpeningStockNo,
                            ItemId = o.ItemId,
                            ActualStock = o.ActualStock,
                            PhysicalStock = o.PhysicalStock,
                            PreparedBy = o.PreparedBy,
                            Remarks = o.Remarks,
                            ItemName = i.ItemName,
                            Unit = i.Unit
                        }).OrderByDescending(x => x.OpeningStockId).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult Getopeningstock(int OpeningStockId)
        {
            var list = (from o in db.OpeningStock
                        join i in db.Items on o.ItemId equals i.ItemId
                        where o.IsActive == 1 && o.OpeningStockId == OpeningStockId
                        select new
                        {
                            OpeningStockId = o.OpeningStockId,
                            AccountYear = o.AccountYear,
                            IncrementId = o.IncrementId,
                            OpeningStockNo = o.OpeningStockNo,
                            ItemId = o.ItemId,
                            ActualStock = o.ActualStock,
                            PhysicalStock = o.PhysicalStock,
                            PreparedBy = o.PreparedBy,
                            Remarks = o.Remarks,
                            ItemName = i.ItemName,
                            Unit = i.Unit
                        }).FirstOrDefault();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual JsonResult Deleteopeningstock(int OpeningStockId)
        {
            bool result = false;
            OpeningStock OpeningStock = db.OpeningStock.Where(x => x.OpeningStockId == OpeningStockId).FirstOrDefault();
            if (OpeningStock != null)
            {
                OpeningStock.IsActive = 0;
                OpeningStock.Modifiedon = DateTime.Now;
                db.SaveChanges();
                db.proc_ItemOpeningStock("Delete", OpeningStock.OpeningStockNo, OpeningStock.Createdon, OpeningStock.ItemId, OpeningStock.ActualStock);
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /* GRN End*/

        [HttpPost]
        public virtual JsonResult SaveGRN(SaveGRNModel model)
        {
            var result = false;
            var isQtyValid = true;
            try
            {
                if (model.GRNModel.GRNId > 0)
                {
                    var pendingPoList = db.ViwPendingGRNPO.Where(x => x.POId == model.GRNModel.POId).ToList();
                    var existSubItemList = db.SubGRN.Where(x => x.GRNId == model.GRNModel.GRNId && x.IsActive == 1).ToList();

                    foreach (var objsubitem in model.GRNSubItemList)
                    {
                        var OldItem = existSubItemList.Where(x => x.ItemId == objsubitem.ItemId).FirstOrDefault();
                        decimal oldQty = 0;
                        if (OldItem != null)
                        {
                            oldQty = OldItem.Quantity;
                        }
                        var existCount = pendingPoList.Where(x => x.ItemId == objsubitem.ItemId && (x.Quantity + oldQty) < objsubitem.Quantity).ToList().Count;
                        if (existCount > 0)
                        {
                            isQtyValid = false;
                        }
                    }
                    if (!isQtyValid)
                    {
                        return Json(new { success = false, message = "GRN Qty should not greater than PO Qty." }, JsonRequestBehavior.AllowGet);
                    }

                    var grnModel = db.GRN.Where(x => x.GRNId == model.GRNModel.GRNId).FirstOrDefault();
                    grnModel.GRNId = model.GRNModel.GRNId;
                    grnModel.GRNDate = model.GRNModel.GRNDate;
                    grnModel.POId = model.GRNModel.POId;
                    grnModel.PODate = model.GRNModel.PODate;
                    grnModel.GateInwardDate = model.GRNModel.GateInwardDate;
                    grnModel.SupplierId = model.GRNModel.SupplierId;
                    grnModel.RequestBy = model.GRNModel.RequestBy;
                    grnModel.GRNType = model.GRNModel.GRNType;
                    grnModel.SubTotal = model.GRNModel.SubTotal;
                    grnModel.CGST = model.GRNModel.CGST;
                    grnModel.SGST = model.GRNModel.SGST;
                    grnModel.IGST = model.GRNModel.IGST;
                    grnModel.Total = model.GRNModel.Total;
                    grnModel.SuppInvoiceNo = model.GRNModel.SuppInvoiceNo;
                    grnModel.SuppInvoiceDate = model.GRNModel.SuppInvoiceDate;
                    grnModel.SuppInvoiceFile = model.GRNModel.SuppInvoiceFile;
                    grnModel.Modifiedby = 1;
                    grnModel.Modifiedon = DateTime.Now;


                    existSubItemList.ForEach(x => x.IsActive = 0);
                    existSubItemList.ForEach(x => x.Modifiedby = 1);
                    existSubItemList.ForEach(x => x.Modifiedon = DateTime.Now);
                    model.GRNSubItemList.ForEach(x => x.GRNId = model.GRNModel.GRNId);
                    model.GRNSubItemList.ForEach(x => x.Createdby = 1);
                    model.GRNSubItemList.ForEach(x => x.Createdon = DateTime.Now);
                    foreach (var objsubitem in model.GRNSubItemList)
                    {
                        db.SubGRN.Add(objsubitem);
                    }

                    db.SaveChanges();
                    GRNItemStock(model.GRNModel.GRNNO, model.GRNModel.GRNDate, "Update", model.GRNSubItemList);
                    result = true;
                }
                else
                {
                    var pendingPoList = db.ViwPendingGRNPO.Where(x => x.POId == model.GRNModel.POId).ToList();

                    foreach (var objsubitem in model.GRNSubItemList)
                    {
                        var existCount = pendingPoList.Where(x => x.ItemId == objsubitem.ItemId && x.Quantity < objsubitem.Quantity).ToList().Count;
                        if (existCount > 0)
                        {
                            isQtyValid = false;
                        }
                    }
                    if (!isQtyValid)
                    {
                        return Json(new { success = false, message = "GRN Qty should not greater than PO Qty." }, JsonRequestBehavior.AllowGet);
                    }

                    int IncNo = 0;
                    string GRNNo = "";
                    GetGRNNo(out IncNo, out GRNNo);
                    model.GRNModel.AccountYear = GetAccountYear();
                    model.GRNModel.GRNNO = GRNNo;
                    model.GRNModel.IncrementId = IncNo;
                    model.GRNModel.IsActive = 1;
                    model.GRNModel.Createdby = 1;
                    model.GRNModel.Createdon = DateTime.Now;
                    db.GRN.Add(model.GRNModel);
                    db.SaveChanges();
                    model.GRNSubItemList.ForEach(x => x.GRNId = model.GRNModel.GRNId);
                    model.GRNSubItemList.ForEach(x => x.Createdby = 1);
                    model.GRNSubItemList.ForEach(x => x.Createdon = DateTime.Now);
                    foreach (var objsubitem in model.GRNSubItemList)
                    {
                        db.SubGRN.Add(objsubitem);
                    }
                    db.SaveChanges();
                    GRNItemStock(model.GRNModel.GRNNO, model.GRNModel.GRNDate, "Add", model.GRNSubItemList);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error in mapping." }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, message = "Saved successfully." }, JsonRequestBehavior.AllowGet);
        }

        public string GRNItemStock(string TranId, DateTime TranDate, string Type, List<SubGRN> subGRN)
        {
            try
            {

                ItemStockCollection itemStock = new ItemStockCollection();
                foreach (var objsubitem in subGRN)
                {
                    itemStock.Add(new ItemOpeningStockType
                    {
                        ItemId = objsubitem.ItemId,
                        TranDate = TranDate,
                        TranId = TranId,
                        Quantity = objsubitem.Quantity
                    });
                }


                SqlParameter[] parameters =
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "list",
                    TypeName = "[dbo].[ItemOpeningStock]",
                    Value = itemStock
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TranId",
                    Value = TranId
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "Type",
                    Value = Type
                }
            };
                db.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, "proc_GRNItemStock @Type,@TranId,@list", parameters);
            }
            catch (Exception e)
            {

            }
            return "";
        }

        public virtual JsonResult GetGRNList()
        {

            var ItemList = (from im in db.GRN
                            join cus in db.Customers on im.SupplierId equals cus.CustomerId
                            where im.IsActive == 1 && cus.IsActive == 1
                            select new GRNModel()
                            {
                                AccountYear = im.AccountYear,
                                CGST = im.CGST,
                                IGST = im.IGST,
                                IncrementId = im.IncrementId,
                                GRNDate = im.GRNDate,
                                GRNId = im.GRNId,
                                GRNNO = im.GRNNO,
                                SGST = im.SGST,
                                SubTotal = im.SubTotal,
                                SupplierId = im.SupplierId,
                                SupplierName = cus.CustomerName,
                                Total = im.Total
                            }).OrderByDescending(x => x.GRNId).ToList();

            return Json(ItemList, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult GetGRNById(int GRNId)
        {

            var GRNModel = (from im in db.GRN
                            join cus in db.Customers on im.SupplierId equals cus.CustomerId
                            join po in db.PurchaseOrder on im.POId equals po.POId
                            where im.IsActive == 1 && cus.IsActive == 1 && im.GRNId == GRNId
                            select new GRNModel()
                            {
                                GRNId = im.GRNId,
                                AccountYear = im.AccountYear,
                                IncrementId = im.IncrementId,
                                GRNNO = im.GRNNO,
                                GRNDate = im.GRNDate,
                                POId = im.POId,
                                PODate = im.PODate,
                                GateInwardDate = im.GateInwardDate,
                                SupplierId = im.SupplierId,
                                SupplierName = cus.CustomerName,
                                PONO = po.PONo,
                                RequestBy = im.RequestBy,
                                GRNType = im.GRNType,
                                SubTotal = im.SubTotal,
                                CGST = im.CGST,
                                SGST = im.SGST,
                                IGST = im.IGST,
                                Total = im.Total,
                                IsActive = im.IsActive,
                                Createdby = im.Createdby,
                                Createdon = im.Createdon,
                                Modifiedby = im.Modifiedby,
                                Modifiedon = im.Modifiedon,
                                SuppInvoiceNo = im.SuppInvoiceNo,
                                SuppInvoiceDate = im.SuppInvoiceDate,
                                SuppInvoiceFile = im.SuppInvoiceFile,
                                CourierCharges = im.CourierCharges
                            }).FirstOrDefault();

            var GRNItemModel = (from im in db.GRN
                                join sub in db.SubGRN on im.GRNId equals sub.GRNId
                                join item in db.Items on sub.ItemId equals item.ItemId
                                where im.IsActive == 1 && sub.IsActive == 1 && im.GRNId == GRNId
                                select new SubGRNModel()
                                {
                                    SubGRNId = sub.SubGRNId,
                                    GRNId = sub.GRNId,
                                    ItemId = sub.ItemId,
                                    ItemName = item.ItemName,
                                    Unit = item.Unit,
                                    Quantity = sub.Quantity,
                                    Rate = sub.Rate,
                                    CGSTPer = sub.CGSTPer,
                                    CGSTAmount = sub.CGSTAmount,
                                    SGSTPer = sub.SGSTPer,
                                    SGSTAmount = sub.SGSTAmount,
                                    IGSTPer = sub.IGSTPer,
                                    IGSTAmount = sub.IGSTAmount,
                                    Total = sub.Total
                                }).ToList();
            var data = new { GRNModel = GRNModel, GRNItemModel = GRNItemModel };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual JsonResult DeleteGRN(int GRNId)
        {
            bool result = false;
            GRN po = db.GRN.Where(x => x.GRNId == GRNId).FirstOrDefault();
            var list = db.SubGRN.Where(x => x.GRNId == GRNId).ToList();
            if (po != null)
            {
                po.IsActive = 0;
                po.Modifiedon = DateTime.Now;
                db.SaveChanges();
                GRNItemStock(po.GRNNO, po.GRNDate, "Delete", list);
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult GetCurrentGRNNo()
        {
            int IncNo = 0;
            string GRNNo = "";
            GetGRNNo(out IncNo, out GRNNo);
            return Json(GRNNo, JsonRequestBehavior.AllowGet);
        }

        public void GetGRNNo(out int IncrementId, out string GRNNo)
        {
            var accountYear = db.getAccountYear().FirstOrDefault().FYEAR;

            ;
            int? maxid = db.GRN.Where(x => x.AccountYear == accountYear).Max(u => (int?)u.IncrementId);
            if (maxid == null)
            {
                IncrementId = 1;
            }
            else
            {
                IncrementId = maxid.Value;
                IncrementId += 1;
            }
            GRNNo = "GRN" + IncrementId.ToString("D4") + "/" + accountYear;
        }

        public virtual JsonResult GetPendingGRNPO(string prefix)
        {
            var list = db.proc_getPendingGRNPO(prefix).Select(x => new
            {
                POId = x.POId,
                PODate = x.PODate.ToString("MM/dd/yyyy"),
                PONo = x.PONo,
                SupplierId = x.SupplierId,
                SupplierName = x.SupplierName
            }).Distinct().ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult GetPendingGRNPOItem(int POId, string prefix)
        {
            var list = db.proc_getPendingGRNPO(prefix).Where(x => x.POId == POId).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /*GRN End*/

        /* Invoice Start*/

        [HttpPost]
        public virtual JsonResult SaveInvoice(SaveInvoiceModel model)
        {
            var result = false;
            var isQtyValid = true;
            try
            {
                if (model.InvoiceModel.InvoiceId > 0)
                {
                    var lineNo = 0;
                    var existSubItemList = db.SubInvoice.Where(x => x.InvoiceId == model.InvoiceModel.InvoiceId && x.IsActive == 1).ToList();

                    foreach (var objsubitem in model.InvoiceSubItemList)
                    {
                        var OldItem = existSubItemList.Where(x => x.ItemId == objsubitem.ItemId).FirstOrDefault();
                        decimal oldQty = 0;
                        if (OldItem != null)
                        {
                            oldQty = OldItem.Quantity;
                        }
                        var existCount = db.ViwItemStock.Where(x => x.ItemId == objsubitem.ItemId && (x.Quantity + oldQty) < objsubitem.Quantity).ToList().Count;
                        if (existCount > 0)
                        {
                            isQtyValid = false;
                            lineNo++;
                            break;
                        }
                    }
                    if (!isQtyValid)
                    {
                        return Json(new { success = false, message = "Quantity should not greater than stock quantity in line No: " + lineNo }, JsonRequestBehavior.AllowGet);
                    }

                    var grnModel = db.Invoice.Where(x => x.InvoiceId == model.InvoiceModel.InvoiceId).FirstOrDefault();

                    grnModel.InvoiceDate = model.InvoiceModel.InvoiceDate;
                    grnModel.Location = model.InvoiceModel.Location;
                    grnModel.Taxable = model.InvoiceModel.Taxable;
                    grnModel.DCNo = model.InvoiceModel.DCNo;
                    grnModel.DCDate = model.InvoiceModel.DCDate;
                    grnModel.CustomerId = model.InvoiceModel.CustomerId;
                    grnModel.Currency = model.InvoiceModel.Currency;
                    grnModel.ExchangeRate = model.InvoiceModel.ExchangeRate;
                    grnModel.PlaceOfSupply = model.InvoiceModel.PlaceOfSupply;
                    grnModel.ApprovalDate = model.InvoiceModel.ApprovalDate;
                    grnModel.PaymentType = model.InvoiceModel.PaymentType;
                    grnModel.Transport = model.InvoiceModel.Transport;
                    grnModel.PreparedBy = model.InvoiceModel.PreparedBy;
                    grnModel.Remarks = model.InvoiceModel.Remarks;
                    grnModel.AdvanceAmount = model.InvoiceModel.AdvanceAmount;
                    grnModel.BalanceAmount = model.InvoiceModel.BalanceAmount;
                    grnModel.DiscountPer = model.InvoiceModel.DiscountPer;
                    grnModel.DiscountAmount = model.InvoiceModel.DiscountAmount;
                    grnModel.SubTotal = model.InvoiceModel.SubTotal;
                    grnModel.CGST = model.InvoiceModel.CGST;
                    grnModel.SGST = model.InvoiceModel.SGST;
                    grnModel.IGST = model.InvoiceModel.IGST;
                    grnModel.Total = model.InvoiceModel.Total;
                    grnModel.Modifiedby = 1;
                    grnModel.Modifiedon = DateTime.Now;
                    grnModel.CourierCharges = model.InvoiceModel.CourierCharges;


                    existSubItemList.ForEach(x => x.IsActive = 0);
                    existSubItemList.ForEach(x => x.Modifiedby = 1);
                    existSubItemList.ForEach(x => x.Modifiedon = DateTime.Now);
                    model.InvoiceSubItemList.ForEach(x => x.InvoiceId = model.InvoiceModel.InvoiceId);
                    model.InvoiceSubItemList.ForEach(x => x.Createdby = 1);
                    model.InvoiceSubItemList.ForEach(x => x.Createdon = DateTime.Now);
                    foreach (var objsubitem in model.InvoiceSubItemList)
                    {
                        db.SubInvoice.Add(objsubitem);
                    }

                    db.SaveChanges();
                    result = true;
                }
                else
                {

                    var lineNo = 0;
                    foreach (var objsubitem in model.InvoiceSubItemList)
                    {
                        var existCount = db.ViwItemStock.Where(x => x.ItemId == objsubitem.ItemId && x.Quantity < objsubitem.Quantity).ToList().Count;
                        if (existCount > 0)
                        {
                            isQtyValid = false;
                            lineNo++;
                            break;
                        }
                    }
                    if (!isQtyValid)
                    {
                        return Json(new { success = false, message = "Quantity should not greater than stock quantity in line No: " + lineNo }, JsonRequestBehavior.AllowGet);
                    }

                    int IncNo = 0;
                    string InvoiceNo = "";
                    GetInvoiceNo(out IncNo, out InvoiceNo);
                    model.InvoiceModel.AccountYear = GetAccountYear();
                    model.InvoiceModel.InvoiceNO = InvoiceNo;
                    model.InvoiceModel.IncrementId = IncNo;
                    model.InvoiceModel.IsActive = 1;
                    model.InvoiceModel.Createdby = 1;
                    model.InvoiceModel.Createdon = DateTime.Now;
                    db.Invoice.Add(model.InvoiceModel);
                    db.SaveChanges();
                    model.InvoiceSubItemList.ForEach(x => x.InvoiceId = model.InvoiceModel.InvoiceId);
                    model.InvoiceSubItemList.ForEach(x => x.Createdby = 1);
                    model.InvoiceSubItemList.ForEach(x => x.Createdon = DateTime.Now);
                    foreach (var objsubitem in model.InvoiceSubItemList)
                    {
                        db.SubInvoice.Add(objsubitem);
                    }
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error in mapping." }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, message = "Saved successfully." }, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult GetInvoiceList()
        {

            var ItemList = (from im in db.Invoice
                            join cus in db.Customers on im.CustomerId equals cus.CustomerId
                            where im.IsActive == 1 && cus.IsActive == 1
                            select new InvoiceModel()
                            {
                                AccountYear = im.AccountYear,
                                CGST = im.CGST,
                                IGST = im.IGST,
                                IncrementId = im.IncrementId,
                                InvoiceDate = im.InvoiceDate,
                                InvoiceId = im.InvoiceId,
                                InvoiceNO = im.InvoiceNO,
                                SGST = im.SGST,
                                SubTotal = im.SubTotal,
                                CustomerId = im.CustomerId,
                                CustomerName = cus.CustomerName,
                                Total = im.Total
                            }).OrderByDescending(x => x.InvoiceId).ToList();

            return Json(ItemList, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult GetInvoiceById(int InvoiceId)
        {

            var InvoiceModel = (from im in db.Invoice
                                join cus in db.Customers on im.CustomerId equals cus.CustomerId
                                where im.IsActive == 1 && cus.IsActive == 1 && im.InvoiceId == InvoiceId
                                select new InvoiceModel()
                                {
                                    InvoiceId = im.InvoiceId,
                                    AccountYear = im.AccountYear,
                                    IncrementId = im.IncrementId,
                                    InvoiceNO = im.InvoiceNO,
                                    InvoiceDate = im.InvoiceDate,
                                    Location = im.Location,
                                    Taxable = im.Taxable,
                                    DCNo = im.DCNo,
                                    DCDate = im.DCDate,
                                    CustomerId = im.CustomerId,
                                    Currency = im.Currency,
                                    ExchangeRate = im.ExchangeRate,
                                    PlaceOfSupply = im.PlaceOfSupply,
                                    ApprovalDate = im.ApprovalDate,
                                    PaymentType = im.PaymentType,
                                    Transport = im.Transport,
                                    PreparedBy = im.PreparedBy,
                                    Remarks = im.Remarks,
                                    AdvanceAmount = im.AdvanceAmount,
                                    BalanceAmount = im.BalanceAmount,
                                    DiscountPer = im.DiscountPer,
                                    DiscountAmount = im.DiscountAmount,
                                    SubTotal = im.SubTotal,
                                    CGST = im.CGST,
                                    SGST = im.SGST,
                                    IGST = im.IGST,
                                    Total = im.Total,
                                    IsActive = im.IsActive,
                                    Createdby = im.Createdby,
                                    Createdon = im.Createdon,
                                    Modifiedby = im.Modifiedby,
                                    Modifiedon = im.Modifiedon,
                                    CourierCharges = im.CourierCharges
                                }).FirstOrDefault();

            var InvoiceItemModel = (from im in db.Invoice
                                    join sub in db.SubInvoice on im.InvoiceId equals sub.InvoiceId
                                    join item in db.Items on sub.ItemId equals item.ItemId
                                    where im.IsActive == 1 && sub.IsActive == 1 && im.InvoiceId == InvoiceId
                                    select new SubInvoiceModel()
                                    {
                                        SubInvoiceId = sub.InvoiceId,
                                        InvoiceId = sub.InvoiceId,
                                        ItemId = sub.ItemId,
                                        ItemName = item.ItemName,
                                        Unit = item.Unit,
                                        Quantity = sub.Quantity,
                                        Rate = sub.Rate,
                                        CGSTPer = sub.CGSTPer,
                                        CGSTAmount = sub.CGSTAmount,
                                        SGSTPer = sub.SGSTPer,
                                        SGSTAmount = sub.SGSTAmount,
                                        IGSTPer = sub.IGSTPer,
                                        IGSTAmount = sub.IGSTAmount,
                                        DiscountPer = sub.DiscountPer,
                                        DiscountAmount = sub.DiscountAmount,
                                        Total = sub.Total
                                    }).ToList();
            var data = new { InvoiceModel = InvoiceModel, InvoiceItemModel = InvoiceItemModel };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual JsonResult DeleteInvoice(int InvoiceId)
        {
            bool result = false;
            Invoice po = db.Invoice.Where(x => x.InvoiceId == InvoiceId).FirstOrDefault();
            if (po != null)
            {
                po.IsActive = 0;
                po.Modifiedon = DateTime.Now;
                db.SaveChanges();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult GetCurrentInvoiceNo()
        {
            int IncNo = 0;
            string InvoiceNo = "";
            GetInvoiceNo(out IncNo, out InvoiceNo);
            return Json(InvoiceNo, JsonRequestBehavior.AllowGet);
        }

        public void GetInvoiceNo(out int IncrementId, out string InvoiceNo)
        {
            var accountYear = db.getAccountYear().FirstOrDefault().FYEAR;

            ;
            int? maxid = db.Invoice.Where(x => x.AccountYear == accountYear).Max(u => (int?)u.IncrementId);
            if (maxid == null)
            {
                IncrementId = 1;
            }
            else
            {
                IncrementId = maxid.Value;
                IncrementId += 1;
            }
            InvoiceNo = "INV" + IncrementId.ToString("D4") + "/" + accountYear;
        }

        public virtual JsonResult GetItemStockItem(int CustomerId, string prefix)
        {
            var list = (from s in db.ViwItemStock
                        join i in db.Items on s.ItemId equals i.ItemId
                        join si in db.SubItemMappings on i.ItemId equals si.ItemId
                        join im in db.ItemMappings on si.ItemMappingId equals im.ItemMappingId
                        where s.Quantity > 0 && i.IsActive == 1 && im.IsActive == 1 && si.IsActive == 1
                        && im.CustomerId == CustomerId
                        && (i.ItemName.Contains(prefix) || i.ItemCode.Contains(prefix))
                        select new
                        {
                            ItemId = i.ItemId,
                            ItemCode = i.ItemCode,
                            ItemName = i.ItemName,
                            Unit = i.Unit,
                            Quantity = s.Quantity
                        }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /*GRN End*/

    }


}
