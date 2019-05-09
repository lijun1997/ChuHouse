using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ch16.Models
{
    public class CVMOrderReport
    {
        public List<OrderReport> orre { get; set; }
        public List<DetailMenu> menu { get; set; }
    }
}