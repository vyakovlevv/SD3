namespace ApiGateway.Services;

public interface IProxyService
{
    Task<IResult> ForwardGet(string path);
    Task<IResult> ForwardPost(string path, HttpRequest request);

    Task<IResult> ForwardPostJson<T>(string path, T body);
}