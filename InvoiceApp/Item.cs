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
    
    public partial class Item
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Item()
        {
            this.ItemStockTran = new HashSet<ItemStockTran>();
            this.OpeningStock = new HashSet<OpeningStock>();
            this.SubGRN = new HashSet<SubGRN>();
            this.SubPurchaseOrder = new HashSet<SubPurchaseOrder>();
            this.ItemStock = new HashSet<ItemStock>();
            this.SubInvoice = new HashSet<SubInvoice>();
        }
    
        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public int IncrementId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public decimal Rate { get; set; }
        public int IsActive { get; set; }
        public int Createdby { get; set; }
        public System.DateTime Createdon { get; set; }
        public Nullable<int> Modifiedby { get; set; }
        public Nullable<System.DateTime> Modifiedon { get; set; }
        public string HSNCode { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ItemStockTran> ItemStockTran { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OpeningStock> OpeningStock { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubGRN> SubGRN { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubPurchaseOrder> SubPurchaseOrder { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ItemStock> ItemStock { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubInvoice> SubInvoice { get; set; }
    }
}
