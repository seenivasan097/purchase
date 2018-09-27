using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS.UI.Areas.Master.Models
{
    public class ItemMappingModel
    {
        public ItemMapping itemMappingModel { get; set; }
        public List<SubItemMapping> subItemMappingList { get; set; }
    }
}