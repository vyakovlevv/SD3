namespace ApiGateway.DTO;

public class AddSubmissionRequestDto
{
    public Guid Student { get; set; }
    public Guid TaskId { get; set; }
    public IFormFile File { get; set; } = null!;
};