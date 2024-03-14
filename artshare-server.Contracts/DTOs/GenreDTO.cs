using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.ApiModels.DTOs
{
    public class GenreDTO
    {
        [Required]
        public string Name { get; set; }
    }

    public class CreateGenreDTO : GenreDTO
    {

    }

    public class UpdateGenreDTO : GenreDTO
    {

    }

    public class GetGenreDTO : GenreDTO
    {
        public int GenreId { get; set; }
    }
}
