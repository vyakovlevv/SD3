namespace FileAnalysisService.Application.DTO;

public class ReportDto
{
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public Guid SubmissionId { get; set; }
    public string ContentHash { get; set; } = string.Empty;
    public bool IsPlagiarism { get; set; }
    public List<Guid> MatchedSubmissionIds { get; set; } = new();
    public List<Guid> MatchedStudentIds { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}