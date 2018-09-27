using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS.UI.Areas.Transaction.Models
{
    public class SavePOModel
    {
        public PurchaseOrder POModel { get; set; }
        public List<SubPurchaseOrder> POSubItemList { get; set; }
    }
}