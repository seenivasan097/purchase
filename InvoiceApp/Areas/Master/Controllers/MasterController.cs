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
using HRMS.UI.Areas.Master.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace InvoiceApp.Areas.Master.Controllers
{
    public partial class MasterController : ApplicationBaseController
    {


        #region "Private variables"

        private readonly ILogger _logger;
        ApplicationEntities db;


        #endregion "Private variables"
        #region "Public variables"

        public MasterController(ILogger logger)
        {
            _logger = logger;
            db = new ApplicationEntities();
        }
        #endregion "Public variables"

        [AllowAnonymous()]
        public virtual ActionResult Customer()
        {
            return View();
        }
        [AllowAnonymous()]
        public virtual ActionResult CustomerList()
        {
            return View();
        }
        public virtual ActionResult CustomerMapping()
        {
            return View();
        }
        public virtual ActionResult ItemList()
        {
            return View();
        }
        public virtual ActionResult StockDetails()
        {
            return View();
        }
        public virtual JsonResult GetCustomerList()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var CustomerList = db.Customers.Where(c => c.IsActive == 1).OrderByDescending(x => x.IncrementId).ToList();

            return Json(CustomerList, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult GetCustomer(int CustomerId)
        {
            Customer model = db.Customers.Where(x => x.CustomerId == CustomerId).FirstOrDefault();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult SaveCustomer(Customer model)
        {
            var result = false;
            int IncrementId = 0;
            string CustCode = "";
            try
            {
                var exist = db.Customers.Where(x => x.CustomerName.ToLower() == model.CustomerName.ToLower() && x.CustomerId != model.CustomerId).ToList().Count;
                if (exist > 0)
                {
                    return Json(new { success = false, message = "Customer name is already exist." }, JsonRequestBehavior.AllowGet);
                }
                if (model.CustomerId > 0)
                {
                    Customer customer = db.Customers.Where(x => x.CustomerId == model.CustomerId).FirstOrDefault();
                    customer.CustomerName = model.CustomerName;
                    customer.Address1 = model.Address1;
                    customer.Address2 = model.Address2;
                    customer.Address3 = model.Address3;
                    customer.MobileNo = model.MobileNo;
                    customer.PhoneNo = model.PhoneNo;
                    customer.ContactPerson = model.ContactPerson;
                    customer.Email = model.Email;
                    customer.AltNo1 = model.AltNo1;
                    customer.AltNo2 = model.AltNo2;
                    customer.AltNo3 = model.AltNo3;
                    customer.CustomerType = model.CustomerType;
                    if (customer.CustomerType != model.CustomerType)
                    {
                        GetCustCode(out IncrementId, out CustCode, model.CustomerType);
                        customer.IncrementId = IncrementId;
                        customer.CustomerCode = CustCode;
                    }
                    customer.GSTNo = model.GSTNo;
                    customer.FaxNo = model.FaxNo;
                    customer.Modifiedby = 1;
                    customer.Modifiedon = DateTime.Now;
                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    GetCustCode(out IncrementId, out CustCode, model.CustomerType);
                    model.IncrementId = IncrementId;
                    model.CustomerCode = CustCode;
                    model.IsActive = 1;
                    model.Createdby = 1;
                    model.Createdon = DateTime.Now;
                    db.Customers.Add(model);
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error in save." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = result, message = "Saved successfully." }, JsonRequestBehavior.AllowGet);
        }

        public void GetCustCode(out int IncrementId, out string CustCode, string CustType)
        {
            int? maxid = db.Customers.Where(x => x.CustomerType == CustType).Max(u => (int?)u.IncrementId);
            if (maxid == null)
            {
                IncrementId = 1;
            }
            else
            {
                IncrementId = maxid.Value;
                IncrementId += 1;
            }
            if (CustType == "Customer")
            {
                CustCode = "CUS" + IncrementId.ToString("D4");
            }
            else
            {
                CustCode = "SUP" + IncrementId.ToString("D4");
            }
        }

        public virtual JsonResult DeleteCustomer(int customerID)
        {
            var count = db.ItemMappings.Where(x => x.CustomerId == customerID && x.IsActive == 1).ToList().Count;
            if (count > 0)
            {
                return Json(new { success = false, message = "This customer/supplier is linked with item mapping." }, JsonRequestBehavior.AllowGet);
            }
            Customer customer = db.Customers.Where(x => x.CustomerId == customerID).FirstOrDefault();
            if (customer != null)
            {
                customer.IsActive = 0;
                customer.Modifiedon = DateTime.Now;
                db.SaveChanges();
            }

            return Json(new { success = true, message = "Deleted Successfully." }, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult GetItemList()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var ItemList = db.Items.Where(c => c.IsActive == 1).OrderByDescending(x => x.ItemId).ToList();

            return Json(ItemList, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult ItemSearch(string prefix)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var ItemList = db.Items.Where(c => c.IsActive == 1 && c.ItemName.Contains(prefix)).ToList();

            return Json(ItemList, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult GetItem(int itemId)
        {
            Item model = db.Items.Where(x => x.ItemId == itemId).FirstOrDefault();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult SaveItem(Item model)
        {
            var result = false;
            try
            {
                if (model.Description == null)
                {
                    model.Description = "";
                }
                var existcode = db.Items.Where(x => x.ItemCode.ToLower() == model.ItemCode.ToLower() && x.ItemId != model.ItemId).ToList().Count;
                if (existcode > 0)
                {
                    return Json(new { success = false, message = "Item code is already exist." }, JsonRequestBehavior.AllowGet);
                }
                var existName = db.Items.Where(x => x.ItemCode.ToLower() == model.ItemCode.ToLower() && x.ItemId != model.ItemId).ToList().Count;
                if (existName > 0)
                {
                    return Json(new { success = false, message = "Item name is already exist." }, JsonRequestBehavior.AllowGet);
                }
                if (model.ItemId > 0)
                {
                    Item item = db.Items.Where(x => x.ItemId == model.ItemId).FirstOrDefault();
                    item.ItemName = model.ItemName;
                    item.Description = model.Description;
                    item.Unit = model.Unit;
                    item.Rate = model.Rate;
                    item.HSNCode = model.HSNCode;
                    item.Modifiedby = 1;
                    item.Modifiedon = DateTime.Now;
                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    int? maxid = db.Items.Max(u => (int?)u.IncrementId);
                    if (maxid == null)
                    {
                        model.IncrementId = 1;
                    }
                    else
                    {
                        model.IncrementId = maxid.Value;
                        model.IncrementId += 1;
                    }
                    model.IncrementId = model.IncrementId;
                    model.IsActive = 1;
                    model.Createdby = 1;
                    model.Createdon = DateTime.Now;
                    db.Items.Add(model);
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error in save." }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, message = "Saved successfully." }, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult DeleteItem(int itemID)
        {
            var count = (from i in db.ItemMappings
                         join si in db.SubItemMappings on i.ItemMappingId equals si.ItemMappingId
                         where i.IsActive == 1 && si.IsActive == 1 && si.ItemId == itemID
                         select i).ToList().Count;
            if (count > 0)
            {
                return Json(new { success = false, message = "This item is linked with item mapping." }, JsonRequestBehavior.AllowGet);
            }
            Item item = db.Items.Where(x => x.ItemId == itemID).FirstOrDefault();
            if (item != null)
            {
                item.IsActive = 0;
                item.Modifiedon = DateTime.Now;
                db.SaveChanges();
            }

            return Json(new { success = true, message = "Deleted Successfully." }, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult getUserList(string type, string prefix)
        {
            var users = (from x in db.Customers
                         where x.IsActive == 1 && x.CustomerType == type && x.CustomerName.Contains(prefix)
                         select new UserModel()
                         {
                             CustomerId = x.CustomerId,
                             CustomerCode = x.CustomerCode,
                             CustomerName = x.CustomerName
                         }).ToList();
            return Json(users, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult getAutocomplteItemList(string prefix)
        {
            var items = (from x in db.Items
                         where x.IsActive == 1 && x.ItemName.Contains(prefix)
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

        public virtual JsonResult SaveItemMapping(ItemMappingModel model)
        {
            var result = false;
            try
            {
                var exist = db.ItemMappings.Where(x => x.CustomerId == model.itemMappingModel.CustomerId && x.ItemMappingId != model.itemMappingModel.ItemMappingId).ToList().Count;
                if (exist > 0)
                {
                    return Json(new { success = false, message = "This " + model.itemMappingModel.CustomerType.ToLower() + " is already mapped with other items." }, JsonRequestBehavior.AllowGet);
                }
                if (model.itemMappingModel.ItemMappingId > 0)
                {
                    var existSubItemList = db.SubItemMappings.Where(x => x.ItemMappingId == model.itemMappingModel.ItemMappingId).ToList();
                    existSubItemList.ForEach(x => x.IsActive = 0);
                    existSubItemList.ForEach(x => x.Modifiedby = 1);
                    existSubItemList.ForEach(x => x.Modifiedon = DateTime.Now);
                    model.subItemMappingList.ForEach(x => x.ItemMappingId = model.itemMappingModel.ItemMappingId);
                    model.subItemMappingList.ForEach(x => x.Createdby = 1);
                    model.subItemMappingList.ForEach(x => x.Createdon = DateTime.Now);
                    foreach (var objsubitem in model.subItemMappingList)
                    {
                        if (objsubitem.Package == null)
                        {
                            objsubitem.Package = "";
                        }
                        db.SubItemMappings.Add(objsubitem);
                    }
                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    model.itemMappingModel.IsActive = 1;
                    model.itemMappingModel.Createdby = 1;
                    model.itemMappingModel.Createdon = DateTime.Now;
                    db.ItemMappings.Add(model.itemMappingModel);
                    db.SaveChanges();
                    model.subItemMappingList.ForEach(x => x.ItemMappingId = model.itemMappingModel.ItemMappingId);
                    model.subItemMappingList.ForEach(x => x.Createdby = 1);
                    model.subItemMappingList.ForEach(x => x.Createdon = DateTime.Now);
                    foreach (var objsubitem in model.subItemMappingList)
                    {
                        if (objsubitem.Package == null)
                        {
                            objsubitem.Package = "";
                        }
                        db.SubItemMappings.Add(objsubitem);
                    }
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error in mapping." }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, message = "Item mapped successfully." }, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult GetItemMapList()
        {

            var ItemList = (from im in db.ItemMappings
                            join cus in db.Customers on im.CustomerId equals cus.CustomerId
                            where im.IsActive == 1 && cus.IsActive == 1
                            select new
                            {
                                CustomerId = im.CustomerId,
                                CustomerType = im.CustomerType,
                                CustomerName = cus.CustomerName,
                                ItemMappingId = im.ItemMappingId,
                                ItemCount = db.SubItemMappings.Where(x => x.ItemMappingId == im.ItemMappingId && x.IsActive == 1).ToList().Count()
                            }).ToList();

            return Json(ItemList, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult DeleteItemMapping(int itemID)
        {
            bool result = false;
            ItemMapping item = db.ItemMappings.Where(x => x.ItemMappingId == itemID).FirstOrDefault();

            if (item != null)
            {
                item.IsActive = 0;
                item.Modifiedon = DateTime.Now;
                db.SaveChanges();
                result = true;
            }
            var list = db.SubItemMappings.Where(x => x.ItemMappingId == itemID).ToList();
            list.ForEach(x => x.IsActive = 0);
            db.SaveChanges();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult GetItemMapping(int itemMappingId)
        {
            var list = new EditItemMappingModel();
            var model = (from im in db.ItemMappings
                         join cus in db.Customers on im.CustomerId equals cus.CustomerId
                         where im.ItemMappingId == itemMappingId
                         select new EditItemMapping()
                         {
                             CustomerId = im.CustomerId,
                             CustomerName = cus.CustomerName,
                             CustomerType = cus.CustomerType,
                             ItemMappingId = im.ItemMappingId
                         }).FirstOrDefault();
            var subitem = (from si in db.SubItemMappings
                           join i in db.Items on si.ItemId equals i.ItemId
                           where si.IsActive == 1 && si.ItemMappingId == itemMappingId
                           select new EditItemSubMappingModel()
                           {
                               CustomerItemCode = si.CustomerItemCode,
                               ItemId = si.ItemId,
                               ItemMappingId = si.ItemMappingId,
                               ItemName = i.ItemName,
                               Package = si.Package,
                               Rate = si.Rate,
                               SubItemMappingId = si.SubItemMappingId,
                               Unit = i.Unit
                           }).ToList();
            list.itemMapping = model;
            list.subItemMappingList = subitem;
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }

}