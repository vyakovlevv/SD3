using ApiGateway.Services;

namespace ApiGateway.Gateways;

public class FileAnalysisGateway
{
    private readonly ProxyService _proxy;

    public FileAnalysisGateway(ProxyService proxy)
    {
        _proxy = proxy;
    }

    public Task<IResult> Analyze(Guid submissionId)
        => _proxy.ForwardPost($"/analyze/{submissionId}", null);

    public Task<IResult> GetReport(Guid submissionId)
        => _proxy.ForwardGet($"/reports/{submissionId}");

    public Task<IResult> GetReports()
        => _proxy.ForwardGet("/reports");
}