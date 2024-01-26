using Microsoft.AspNetCore.Http;

namespace RawCoding.EndpointPdk;

public interface IPluginEndpoint
{
    Task ExecuteAsync(HttpContext context);
}