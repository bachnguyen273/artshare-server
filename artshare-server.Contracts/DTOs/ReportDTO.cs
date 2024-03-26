using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.ApiModels.DTOs
{
    public class ReportDTO
    {
        public int AccountId { get; set; }
        public int ArtworkId { get; set; }
        public string Content { get; set; }
        public string Category { get; set; }
        public DateTime ReportDate { get; set; }
        public string Status { get; set; }
    }

    public class CreateReportDTO : ReportDTO
    {

    }

    public class GetReportDTO : ReportDTO
    {
        public int ReportId { get; set; }
        //public GetArtworkDTO Artwork { get; set; }
        //public GetAccountDTO Account { get; set; }
    }

    public class UpdateReportDTO : ReportDTO
    {

    }
}
