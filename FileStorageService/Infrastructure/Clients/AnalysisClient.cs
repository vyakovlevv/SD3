using System.Net.Http.Json;

namespace FileStorageService.Infrastructure.Clients;

public class AnalysisClient
{
    private readonly HttpClient _http;

    public AnalysisClient(HttpClient http)
    {
        _http = http;
    }

    public async Task SendForAnalysisAsync(Guid submissionId)
    {
        var response = await _http.PostAsync($"/analyze/{submissionId}", null);

        if (!response.IsSuccessStatusCode)
        {
            var text = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[Analysis ERROR] {response.StatusCode}: {text}");
        }
    }
}