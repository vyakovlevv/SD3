namespace ApiGateway.Services;

public class ProxyService
{
    private readonly HttpClient _http;

    public ProxyService(HttpClient http)
    {
        _http = http;
    }

    public async Task<IResult> ForwardGet(string path)
    {
        var response = await _http.GetAsync(path);

        var content = await response.Content.ReadAsByteArrayAsync();
        return Results.Bytes(content, response.Content.Headers.ContentType?.ToString());
    }

    public async Task<IResult> ForwardPost(string path, HttpRequest request)
    {
        using var content = new StreamContent(request.Body);
        foreach (var header in request.Headers)
            content.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());

        var response = await _http.PostAsync(path, content);

        var respContent = await response.Content.ReadAsByteArrayAsync();
        return Results.Bytes(respContent, response.Content.Headers.ContentType?.ToString());
    }
}