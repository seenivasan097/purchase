using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS.UI.Areas.Transaction.Models
{
    public class ItemOpeningStockType
    {
        public int ItemId;
        public decimal Quantity;
        public string TranId;
        public DateTime TranDate;
    }
}