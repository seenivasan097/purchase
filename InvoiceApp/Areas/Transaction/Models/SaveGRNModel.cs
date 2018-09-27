using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS.UI.Areas.Transaction.Models
{
    public class SaveGRNModel
    {
        public GRN GRNModel { get; set; }
        public List<SubGRN> GRNSubItemList { get; set; }

    }
}