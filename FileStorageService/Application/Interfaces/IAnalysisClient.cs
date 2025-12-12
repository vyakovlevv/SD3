namespace FileStorageService.Application.Interfaces;

public interface IAnalysisClient
{
    Task SendForAnalysisAsync(Guid submissionId);
}
