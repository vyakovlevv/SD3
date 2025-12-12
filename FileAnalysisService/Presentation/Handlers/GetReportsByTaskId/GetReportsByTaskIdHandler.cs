using FileAnalysisService.Application.Interfaces;
using FileAnalysisService.Presentation.Handlers.GetReportBySubmissionId;

namespace FileAnalysisService.Presentation.Handlers.GetReportsByTaskId;

public class GetReportsByTaskIdHandler(IReportRepository repo) : IGetReportsByTaskIdHandler
{
    public async Task<GetReportsByTaskIdResponse> Handle(Guid taskId)
    {
        var list = await repo.GetAllAsync();
        var dtoList = list.Where(r => r.TaskId == taskId).Select(r => new GetReportResponse
        (
            r.Id,
            r.SubmissionId,
            r.TaskId,
            r.IsPlagiarism,
            System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(r.MatchedStudentIdsJson) ?? new(),
            r.CreatedAt
        )).ToList();
        return new GetReportsByTaskIdResponse(dtoList);
    }
}