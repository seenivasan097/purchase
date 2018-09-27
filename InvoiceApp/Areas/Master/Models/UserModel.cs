using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS.UI.Areas.Master.Models
{
    public class UserModel
    {
        public int CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
    }
}