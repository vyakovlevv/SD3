using FileAnalysisService.Application.Interfaces;

namespace FileAnalysisService.Presentation.Handlers.GetReportBySubmissionId;

public class GetReportBySubmissionIdHandler(IReportRepository repo) : IGetReportBySubmissionIdHandler
{
    public async Task<GetReportResponse?> Handle(Guid submissionId)
    {
        var r = await repo.GetBySubmissionIdAsync(submissionId);
        if (r == null) return null;

        return new (
            r.Id,
            r.SubmissionId,
            r.TaskId,
            r.IsPlagiarism,
            System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(r.MatchedStudentIdsJson) ?? new(),
            r.CreatedAt
        );
    }
}