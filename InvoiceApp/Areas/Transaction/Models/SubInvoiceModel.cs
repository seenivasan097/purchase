using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS.UI.Areas.Transaction.Models
{
    public class SubInvoiceModel
    {
        public int SubInvoiceId { get; set; }
        public int InvoiceId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal CGSTPer { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal SGSTPer { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal IGSTPer { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal DiscountPer { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Total { get; set; }
        public int IsActive { get; set; }
        public int Createdby { get; set; }
        public System.DateTime Createdon { get; set; }
        public Nullable<int> Modifiedby { get; set; }
        public Nullable<System.DateTime> Modifiedon { get; set; }
    }
}