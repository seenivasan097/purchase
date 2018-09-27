using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS.UI.Areas.Transaction.Models
{
    public class PurchaseOrderItemModel
    {
        public int SubPOId { get; set; }
        public int POId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemUnit { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal CGSTPer { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal SGSTPer { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal IGSTPer { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal Total { get; set; }
    }
}