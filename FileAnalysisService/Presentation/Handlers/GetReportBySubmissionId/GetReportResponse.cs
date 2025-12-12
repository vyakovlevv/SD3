namespace FileAnalysisService.Presentation.Handlers.GetReportBySubmissionId;

public record GetReportResponse(Guid Id, Guid TaskId, Guid SubmissionId, bool IsPlagiarism, List<Guid> MatchedStudentIds, DateTime CreatedAt);