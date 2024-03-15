using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Enums;
using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using artshare_server.Services.Interfaces;
using AutoMapper;

namespace artshare_server.Services.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Report>> GetAllReportsAsync()
        {
            var reportList = await _unitOfWork.ReportRepo.GetAllAsync();
            return reportList;
        }

        public async Task<Report?> GetReportByIdAsync(int reportId)
        {
            if (reportId > 0)
            {
                var report = await _unitOfWork.ReportRepo.GetByIdAsync(reportId);
                return report;
            }
            return null;
        }

        public async Task<bool> CreateReportAsync(ReportDTO reportDTO)
        {
            var report = _mapper.Map<Report>(reportDTO);
            report.ReportDate = DateTime.Now;
            report.Status = ReportStatus.Processing;

            await _unitOfWork.ReportRepo.AddAsync(report);
            var result = await _unitOfWork.SaveAsync() > 0;
            return result;
        }

        public async Task<bool> UpdateReportAsync(Report report)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteReportAsync(int reportId)
        {
            throw new NotImplementedException();
        }

        public Task<Report?> GetReportByAccountIdAndArtworkId(int accountId, int artworkId)
        {
            return _unitOfWork.ReportRepo.GetReportByAccountIdAndArtworkId(accountId, artworkId);
        }

        
    }
}