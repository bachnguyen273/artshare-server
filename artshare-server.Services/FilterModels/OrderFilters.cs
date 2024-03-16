using artshare_server.Core.Models;
using artshare_server.Services.FilterModels.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.Services.FilterModels
{
    public class OrderFilters : FilterOptions<Artwork>
    {
        public int? OrderId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}
