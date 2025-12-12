using System.Net.Http.Json;
using FileStorageService.Application.Interfaces;

namespace FileStorageService.Infrastructure.Clients;


public class AnalysisClient : IAnalysisClient
{
    private readonly HttpClient _http;


    public AnalysisClient(HttpClient http)
    {
        _http = http;
    }

    public async Task SendForAnalysisAsync(Guid submissionId)
    {
        var response = await _http.PostAsJsonAsync("/reports/upload",
            new { SubmissionId = submissionId });
        Console.WriteLine(response.StatusCode);

        if (!response.IsSuccessStatusCode)
        {
            var text = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[Analysis ERROR] {response.StatusCode}: {text}");
        }
    }
}