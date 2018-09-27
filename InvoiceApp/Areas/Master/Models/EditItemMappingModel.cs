using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS.UI.Areas.Master.Models
{
    public class EditItemMappingModel
    {
        public EditItemMapping itemMapping { get; set; }
        public List<EditItemSubMappingModel> subItemMappingList { get; set; }
    }

    public class EditItemMapping
    {
        public int ItemMappingId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerType { get; set; }
    }

    public class EditItemSubMappingModel
    {
        public int SubItemMappingId { get; set; }
        public int ItemMappingId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public string CustomerItemCode { get; set; }
        public string Package { get; set; }
        public decimal Rate { get; set; }
    }
}