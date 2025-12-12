using System.Net.Http.Json;
using System.Text.Json;
using FileAnalysisService.Application.DTO;

namespace FileAnalysisService.Infrastructure.Clients;

// DTO, эквивалентный SubmissionDto в FileStorageService
public record FileStorageSubmissionDto(Guid Id, Guid Student, Guid TaskId, string FileName, string StoragePath, DateTime UploadedAt);

public interface IFileStorageClient
{
    Task<(byte[] FileBytes, string FileName)?> GetSubmissionFileAsync(Guid submissionId);

    Task<List<FileStorageSubmissionDto>> GetAllSubmissionsAsync();
}

public class FileStorageClient : IFileStorageClient
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public FileStorageClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<(byte[] FileBytes, string FileName)?> GetSubmissionFileAsync(Guid submissionId)
    {
        var resp = await _http.GetAsync($"/submissions/{submissionId}");
        if (resp.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
        resp.EnsureSuccessStatusCode();
        var bytes = await resp.Content.ReadAsByteArrayAsync();
        var fileName = resp.Content.Headers.ContentDisposition?.FileName?.Trim('"') ?? $"{submissionId}.txt";
        return (bytes, fileName);
    }

    public async Task<List<FileStorageSubmissionDto>> GetAllSubmissionsAsync()
    {
        var resp = await _http.GetAsync("/submissions");
        resp.EnsureSuccessStatusCode();
        var stream = await resp.Content.ReadAsStreamAsync();
        var list = await JsonSerializer.DeserializeAsync<Dictionary<string, List<FileStorageSubmissionDto>>>(stream, _jsonOptions);
        return list.GetValueOrDefault("submissions") ?? new List<FileStorageSubmissionDto>();
    }
}
