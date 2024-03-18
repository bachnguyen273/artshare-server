using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.ApiModels.DTOs
{
    public class Order_OrderDetailsCreateDTO
    {
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public ICollection<OrderDetailsCreateDTO> OrderDetails { get; set; }
    }

    public class OrderDetailsCreateDTO
    {
        public int ArtworkId { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
