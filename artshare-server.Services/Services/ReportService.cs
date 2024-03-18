using artshare_server.ApiModels.DTOs;
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

        public ReportService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetReportDTO>> GetAllReportsAsync()
        {
            var reportList = await _unitOfWork.ReportRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<GetReportDTO>>(reportList);
        }

        public async Task<GetReportDTO?> GetReportByIdAsync(int reportId)
        {
            if (reportId > 0)
            {
                var report = await _unitOfWork.ReportRepo.GetByIdAsync(reportId);
                return _mapper.Map<GetReportDTO>(report);
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