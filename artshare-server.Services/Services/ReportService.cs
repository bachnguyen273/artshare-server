﻿using artshare_server.ApiModels.DTOs;
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

        public ReportService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetReportDTO>> GetAllReportsAsync()
        {
            var reportList = await _unitOfWork.ReportRepo.GetAll();
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

        public async Task<bool> DenyReport(int reportId)
        {
            var report = await _unitOfWork.ReportRepo.GetByIdAsync(reportId);
            if (report != null)
            {
                report.Status = ReportStatus.Processed;
                _unitOfWork.ReportRepo.Update(report);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (result)
                {
                    return true;
                }

            }
            return false;
        }

        public async Task<bool> AcceptReport(int reportId)
        {
            var report = await _unitOfWork.ReportRepo.GetByIdAsync(reportId);
            if (report != null)
            {
                report.Status = ReportStatus.Processed;
                _unitOfWork.ReportRepo.Update(report);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (result)
                {
                    var artwork = await _unitOfWork.ArtworkRepo.GetByIdAsync((int) report.ArtworkId);
                    artwork.Status = ArtworkStatus.Banned;
                    _unitOfWork.ArtworkRepo.Update(artwork);
                    var check = await _unitOfWork.SaveAsync() > 0;
                    if (check)
                        return true;
                }

            }
            return false;
        }
    }
}