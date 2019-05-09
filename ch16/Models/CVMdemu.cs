using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ch16.Models
{
    public class CVMdemu
    {
        public List<StoreReport> sre { get; set; }
        public List<Order> order { get; set; }
        public List<DetailMenu> menu { get; set; }
        public List<OrderDel> del { get; set; }
        
    }
}