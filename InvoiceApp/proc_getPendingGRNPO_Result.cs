//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HRMS.UI
{
    using System;
    
    public partial class proc_getPendingGRNPO_Result
    {
        public Nullable<int> POId { get; set; }
        public System.DateTime PODate { get; set; }
        public string PONo { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public string HSNCode { get; set; }
        public string ItemName { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public string Unit { get; set; }
        public decimal Rate { get; set; }
        public decimal CGSTPer { get; set; }
        public decimal IGSTPer { get; set; }
        public decimal SGSTPer { get; set; }
    }
}