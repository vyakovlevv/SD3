using FileAnalysisService.Presentation.Handlers.GetReportBySubmissionId;

namespace FileAnalysisService.Presentation.Handlers.GetReportsByTaskId;

public interface IGetReportsByTaskIdHandler
{
    Task<GetReportsByTaskIdResponse> Handle(Guid taskId);
}