using ApiGateway.Clients;
using ApiGateway.Services;

namespace ApiGateway.Gateways;

public interface IFileAnalysisGateway
{
    Task<IResult> Analyze(AddReportRequest request);
    Task<IResult> GetReport(Guid submissionId);
    Task<IResult> GetReports(Guid taskId);
}

public class FileAnalysisGateway : IFileAnalysisGateway
{
    private readonly ProxyService _proxy;

    public FileAnalysisGateway(ProxyService proxy)
    {
        _proxy = proxy;
    }

    public Task<IResult> Analyze(AddReportRequest request)
        => _proxy.ForwardPostJson("/reports/upload", request);

    public Task<IResult> GetReport(Guid submissionId)
        => _proxy.ForwardGet($"/reports/submission/{submissionId}");

    public Task<IResult> GetReports(Guid taskId)
        => _proxy.ForwardGet($"/reports/task/{taskId}");
}