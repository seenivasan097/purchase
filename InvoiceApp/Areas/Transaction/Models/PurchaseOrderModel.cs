using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS.UI.Areas.Transaction.Models
{
    public class PurchaseOrderModel
    {
        public int POId { get; set; }
        public System.DateTime PODate { get; set; }
        public string AccountYear { get; set; }
        public int IncrementId { get; set; }
        public string PONo { get; set; }
        public string IntentNo { get; set; }
        public Nullable<System.DateTime> IntentDate { get; set; }
        public string EnquiryNo { get; set; }
        public Nullable<System.DateTime> EnquiryDate { get; set; }
        public string QuotationNo { get; set; }
        public Nullable<System.DateTime> QuotationDate { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public Nullable<System.DateTime> BillDate { get; set; }
        public string RequestedBy { get; set; }
        public string POType { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public decimal Total { get; set; }
        public string DeliverySchedule { get; set; }
        public string DeliveryType { get; set; }
        public int IsActive { get; set; }
        public int Createdby { get; set; }
        public System.DateTime Createdon { get; set; }
        public Nullable<int> Modifiedby { get; set; }
        public Nullable<System.DateTime> Modifiedon { get; set; }
        public Nullable<decimal> CourierCharges { get; set; }
    }
}