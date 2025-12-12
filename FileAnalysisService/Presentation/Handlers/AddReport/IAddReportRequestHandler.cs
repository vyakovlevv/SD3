using FileAnalysisService.Presentation.Handlers.GetReportBySubmissionId;

namespace FileAnalysisService.Presentation.Handlers.AddReport;

public interface IAddReportRequestHandler
{
    Task<GetReportResponse> Handle(AddReportRequest request);
}