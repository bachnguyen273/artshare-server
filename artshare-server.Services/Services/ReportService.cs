using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using artshare_server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.Services.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Report>> GetAllReportsAsync()
        {
            var reportList = await _unitOfWork.ReportRepo.GetAllAsync();
            return reportList;
        }

        public async Task<Report> GetReportByIdAsync(int reportId)
        {
            if (reportId > 0)
            {
                var report = await _unitOfWork.ReportRepo.GetByIdAsync(reportId);
                if (report != null)
                {
                    return report;
                }
            }
            return null;
        }

        public async Task<bool> CreateReportAsync(Report report)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateReportAsync(Report report)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteReportAsync(int reportId)
        {
            throw new NotImplementedException();
        }
    }
}
