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
    
    public partial class ItemStock
    {
        public int ItemStockId { get; set; }
        public int ItemId { get; set; }
        public decimal Quantity { get; set; }
        public string TranId { get; set; }
        public string TranType { get; set; }
    
        public virtual Item Item { get; set; }
    }
}
