using ApiGateway.Services;

namespace ApiGateway.Gateways;



public class FileStorageGateway
{
    private readonly ProxyService _proxy;

    public FileStorageGateway(ProxyService proxy)
    {
        _proxy = proxy;
    }

    public Task<IResult> Upload(HttpRequest request)
        => _proxy.ForwardPost("/submissions", request);

    public Task<IResult> GetSubmission(Guid id)
        => _proxy.ForwardGet($"/submissions/{id}");

    public Task<IResult> GetAllSubmissions()
        => _proxy.ForwardGet("/submissions");
}