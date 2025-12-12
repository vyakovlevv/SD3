namespace FileAnalysisService.Presentation.Handlers.GetReportBySubmissionId;

public interface IGetReportBySubmissionIdHandler
{
    Task<GetReportResponse?> Handle(Guid submissionId);
}