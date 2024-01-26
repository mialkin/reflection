using System.Reflection;
using RawCoding.EndpointPdk;

public class PluginMiddleware
{
    private readonly RequestDelegate _next;

    public PluginMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var path =
            "/Users/aleksei/repositories/reflection/src/RawCoding.TestEndpoint/bin/Debug/net8.0/RawCoding.TestEndpoint.dll";

        var assembly = Assembly.LoadFrom(path);
        var endpointType = assembly.GetType("RawCoding.TestEndpoint.AnEndpoint");
        var pathInfo = endpointType?.GetCustomAttribute<PathAttribute>();
        
        if (
            pathInfo is not null
            && pathInfo.Method.Equals(context.Request.Method, StringComparison.OrdinalIgnoreCase)
            && pathInfo.Path.Equals(context.Request.Path, StringComparison.OrdinalIgnoreCase)
            )
        {
            var endpoint = Activator.CreateInstance(endpointType ?? throw new InvalidOperationException()) as IPluginEndpoint;
            if (endpoint is null)
                throw new InvalidOperationException();
            
            await endpoint.ExecuteAsync(context);
        }

        if (!context.Response.HasStarted)
        {
            await _next.Invoke(context);
        }
    }
}