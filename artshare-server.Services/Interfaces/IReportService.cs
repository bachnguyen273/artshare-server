using artshare_server.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.Services.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<Report>> GetAllReportsAsync();

        Task<Report> GetReportByIdAsync(int reportId);

        Task<bool> CreateReportAsync(Report report);

        Task<bool> UpdateReportAsync(Report report);

        Task<bool> DeleteReportAsync(int reportId);
    }
}
