using artshare_server.Core.Models;
using artshare_server.Services.FilterModels.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.Services.FilterModels
{
    public class GenreFilters : FilterOptions<Genre>
    {
        public int? GenreId { get; set; }
        public string? Name { get; set; }
    }
}
