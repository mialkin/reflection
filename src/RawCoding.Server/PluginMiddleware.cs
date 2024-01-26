public class PluginMiddleware
{
    private readonly RequestDelegate _next;

    public PluginMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path == "/plugin/test")
        {
            await context.Response.WriteAsync("Test");
        }

        if (!context.Response.HasStarted)
        {
            await _next.Invoke(context);
        }
    }
}