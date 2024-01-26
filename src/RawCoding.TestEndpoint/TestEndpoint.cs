using Microsoft.AspNetCore.Http;
using RawCoding.EndpointPdk;

namespace RawCoding.TestEndpoint;

[Path("get", "/plug/test")]
public class TestEndpoint : IPluginEndpoint
{
    public async Task ExecuteAsync(HttpContext context)
    {
        await context.Response.WriteAsync("Test");
    }
}