using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
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
        var reference = await ProcessAsync(context);

        if (!context.Response.HasStarted)
        {
            await _next.Invoke(context);
        }

        for (int i = 0; i < 10 && reference.IsAlive; i++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Console.WriteLine($"Unloading attempt: {i}");
        }

        Console.WriteLine($"Unloading successful: {!reference.IsAlive}");
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static async Task<WeakReference> ProcessAsync(HttpContext context)
    {
        var path =
            "/Users/aleksei/repositories/reflection/src/RawCoding.TestEndpoint/bin/Debug/net8.0/RawCoding.TestEndpoint.dll";

        var loadContext = new AssemblyLoadContext(name: path, isCollectible: true);

        try
        {
            var assembly = loadContext.LoadFromAssemblyPath(path);
            var endpointType = assembly.GetType("RawCoding.TestEndpoint.AnEndpoint");
            var pathInfo = endpointType?.GetCustomAttribute<PathAttribute>();

            if (
                pathInfo is not null
                && pathInfo.Method.Equals(context.Request.Method, StringComparison.OrdinalIgnoreCase)
                && pathInfo.Path.Equals(context.Request.Path, StringComparison.OrdinalIgnoreCase)
            )
            {
                var endpoint =
                    Activator.CreateInstance(endpointType ?? throw new InvalidOperationException()) as IPluginEndpoint;

                if (endpoint is null)
                    throw new InvalidOperationException();

                await endpoint.ExecuteAsync(context);
            }
        }
        finally
        {
            loadContext.Unload();
        }

        return new WeakReference(loadContext);
    }
}