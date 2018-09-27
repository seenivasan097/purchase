﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class ApplicationEntities : DbContext
    {
        public ApplicationEntities()
            : base("name=ApplicationEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ItemMapping> ItemMappings { get; set; }
        public virtual DbSet<SubItemMapping> SubItemMappings { get; set; }
        public virtual DbSet<ItemStockTran> ItemStockTran { get; set; }
        public virtual DbSet<OpeningStock> OpeningStock { get; set; }
        public virtual DbSet<PurchaseOrder> PurchaseOrder { get; set; }
        public virtual DbSet<SubGRN> SubGRN { get; set; }
        public virtual DbSet<SubPurchaseOrder> SubPurchaseOrder { get; set; }
        public virtual DbSet<ItemStock> ItemStock { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<SubInvoice> SubInvoice { get; set; }
        public virtual DbSet<GRN> GRN { get; set; }
        public virtual DbSet<ViwPendingGRNPO> ViwPendingGRNPO { get; set; }
        public virtual DbSet<ViwItemStock> ViwItemStock { get; set; }
    
        public virtual ObjectResult<getAccountYear_Result> getAccountYear()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getAccountYear_Result>("getAccountYear");
        }
    
        public virtual int proc_ItemStock(string type, string tranId)
        {
            var typeParameter = type != null ?
                new ObjectParameter("Type", type) :
                new ObjectParameter("Type", typeof(string));
    
            var tranIdParameter = tranId != null ?
                new ObjectParameter("TranId", tranId) :
                new ObjectParameter("TranId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("proc_ItemStock", typeParameter, tranIdParameter);
        }
    
        public virtual ObjectResult<proc_ItemOpeningStock_Result> proc_ItemOpeningStock(string type, string tranId, Nullable<System.DateTime> tranDate, Nullable<int> itemId, Nullable<decimal> quantity)
        {
            var typeParameter = type != null ?
                new ObjectParameter("Type", type) :
                new ObjectParameter("Type", typeof(string));
    
            var tranIdParameter = tranId != null ?
                new ObjectParameter("TranId", tranId) :
                new ObjectParameter("TranId", typeof(string));
    
            var tranDateParameter = tranDate.HasValue ?
                new ObjectParameter("TranDate", tranDate) :
                new ObjectParameter("TranDate", typeof(System.DateTime));
    
            var itemIdParameter = itemId.HasValue ?
                new ObjectParameter("ItemId", itemId) :
                new ObjectParameter("ItemId", typeof(int));
    
            var quantityParameter = quantity.HasValue ?
                new ObjectParameter("Quantity", quantity) :
                new ObjectParameter("Quantity", typeof(decimal));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_ItemOpeningStock_Result>("proc_ItemOpeningStock", typeParameter, tranIdParameter, tranDateParameter, itemIdParameter, quantityParameter);
        }
    
        public virtual ObjectResult<proc_getPendingGRNPO_Result> proc_getPendingGRNPO(string prefix)
        {
            var prefixParameter = prefix != null ?
                new ObjectParameter("prefix", prefix) :
                new ObjectParameter("prefix", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_getPendingGRNPO_Result>("proc_getPendingGRNPO", prefixParameter);
        }
    }
}