using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.ApiModels.DTOs
{
    public class OrderDashboard
    {
        public int OrderId { get; set; }
        public int? CreatorID { get; set; }
        public string FullName { get; set; }
        public DateTime CreateDate { get; set; }
        public decimal Price { get; set; }
        public string Title { get; set; }
    }
}
