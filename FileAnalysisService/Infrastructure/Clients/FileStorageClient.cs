using System.Net.Http.Json;
using System.Text.Json;
using FileAnalysisService.Application.DTO;

namespace FileAnalysisService.Infrastructure.Clients;

public class FileStorageClient
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
