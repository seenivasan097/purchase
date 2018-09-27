using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS.UI.Areas.Transaction.Models
{
    public class SaveInvoiceModel
    {
        public Invoice InvoiceModel { get; set; }
        public List<SubInvoice> InvoiceSubItemList { get; set; }
    }
}