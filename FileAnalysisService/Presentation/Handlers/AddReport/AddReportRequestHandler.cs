using FileAnalysisService.Application.Interfaces;
using FileAnalysisService.Application.Services;
using FileAnalysisService.Presentation.Handlers.GetReportBySubmissionId;

namespace FileAnalysisService.Presentation.Handlers.AddReport;

public class AddReportRequestHandler(IAnalysisService service) : IAddReportRequestHandler
{
    public async Task<GetReportResponse> Handle(AddReportRequest request)
    {
        var r = await service.AnalyzeSubmissionAsync(request.SubmissionId);
        return new (
            r.Id,
            r.TaskId,
            r.SubmissionId,
            r.IsPlagiarism,
            r.MatchedStudentIds,
            r.CreatedAt
        );
    }
}