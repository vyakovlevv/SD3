namespace FileAnalysisService.Domain.Entities;

public class Report
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid SubmissionId { get; set; }
    
    public Guid TaskId { get; set; }

    public string ContentHash { get; set; } = string.Empty;

    public bool IsPlagiarism { get; set; }

    public string MatchedSubmissionIdsJson { get; set; } = "[]";

    public string MatchedStudentIdsJson { get; set; } = "[]";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}