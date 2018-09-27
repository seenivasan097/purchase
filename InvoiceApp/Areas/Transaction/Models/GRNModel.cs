using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS.UI.Areas.Transaction.Models
{
    public class GRNModel
    {
        public int GRNId { get; set; }
        public string AccountYear { get; set; }
        public int IncrementId { get; set; }
        public string GRNNO { get; set; }
        public System.DateTime GRNDate { get; set; }
        public Nullable<int> POId { get; set; }
        public string PONO { get; set; }
        public Nullable<System.DateTime> PODate { get; set; }
        public Nullable<System.DateTime> GateInwardDate { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string RequestBy { get; set; }
        public string GRNType { get; set; }
        public decimal SubTotal { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public decimal Total { get; set; }
        public int IsActive { get; set; }
        public int Createdby { get; set; }
        public System.DateTime Createdon { get; set; }
        public Nullable<int> Modifiedby { get; set; }
        public Nullable<System.DateTime> Modifiedon { get; set; }
        public string SuppInvoiceNo { get; set; }
        public Nullable<System.DateTime> SuppInvoiceDate { get; set; }
        public string SuppInvoiceFile { get; set; }
        public Nullable<decimal> CourierCharges { get; set; }
    }
}