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
    using System.Collections.Generic;
    
    public partial class PurchaseOrder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PurchaseOrder()
        {
            this.SubPurchaseOrder = new HashSet<SubPurchaseOrder>();
            this.GRN = new HashSet<GRN>();
        }
    
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
    
        public virtual Customer Customer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubPurchaseOrder> SubPurchaseOrder { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GRN> GRN { get; set; }
    }
}
