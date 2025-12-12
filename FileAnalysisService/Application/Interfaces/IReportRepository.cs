using FileAnalysisService.Domain.Entities;

namespace FileAnalysisService.Application.Interfaces;

public interface IReportRepository
{
    Task AddAsync(Report report);
    Task<Report?> GetBySubmissionIdAsync(Guid submissionId);
    Task<List<Report>> GetAllAsync();
    Task<List<Report>> GetByHashAsync(string hash);
}