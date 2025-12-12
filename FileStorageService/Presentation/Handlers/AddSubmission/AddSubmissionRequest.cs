namespace FileStorageService.Presentation.Handlers.AddSubmission;

public class AddSubmissionRequest
{
    public Guid Student { get; set; }
    public Guid TaskId { get; set; }
    public IFormFile File { get; set; } = null!;
}