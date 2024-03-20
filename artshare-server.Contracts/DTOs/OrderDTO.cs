using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.ApiModels.DTOs
{
    public class OrderDTO
    {
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class CreateOrderDTO : OrderDTO
    {

    }

    public class GetOrderDTO : OrderDTO
    {
        public int OrderId { get; set; }
    }
}
