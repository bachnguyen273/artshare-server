﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.ApiModels.DTOs
{
    public class OrderDetailDTO
    {
        public int OrderId { get; set; }
        public int ArtworkId { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
