using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS.UI.Areas.Transaction.Models
{
    public class InvoiceModel
    {
        public int InvoiceId { get; set; }
        public string AccountYear { get; set; }
        public int IncrementId { get; set; }
        public string InvoiceNO { get; set; }
        public System.DateTime InvoiceDate { get; set; }
        public string Location { get; set; }
        public string Taxable { get; set; }
        public string DCNo { get; set; }
        public Nullable<System.DateTime> DCDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Currency { get; set; }
        public decimal ExchangeRate { get; set; }
        public string PlaceOfSupply { get; set; }
        public Nullable<System.DateTime> ApprovalDate { get; set; }
        public string PaymentType { get; set; }
        public string Transport { get; set; }
        public string PreparedBy { get; set; }
        public string Remarks { get; set; }
        public decimal AdvanceAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public decimal SubTotal { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public decimal DiscountPer { get; set; }
        public decimal DiscountAmount { get; set; }
        public Nullable<decimal> CourierCharges { get; set; }
        public decimal Total { get; set; }
        public int IsActive { get; set; }
        public int Createdby { get; set; }
        public System.DateTime Createdon { get; set; }
        public Nullable<int> Modifiedby { get; set; }
        public Nullable<System.DateTime> Modifiedon { get; set; }
        public string DeliveryType { get; set; }
    }
}