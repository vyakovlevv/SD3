using FileAnalysisService.Application.DTO;

namespace FileAnalysisService.Application.Interfaces;

public interface IAnalysisService
{
    Task<ReportDto> AnalyzeSubmissionAsync(Guid submissionId);
}