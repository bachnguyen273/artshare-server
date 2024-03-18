using artshare_server.Core.Enums;
using artshare_server.Core.Models;
using artshare_server.Services.FilterModels.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.Services.FilterModels
{
    public class ArtworkFilters : FilterOptions<Artwork>
    {
        // Filtering criteria properties
        public int? CreatorId { get; set; }
        public string? Title { get; set; }
        public int? GenreId { get; set; }
        public ArtworkStatus? ArtworkStatus { get; set; }
    }
}
