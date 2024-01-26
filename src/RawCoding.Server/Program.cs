var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

var application = builder.Build();

application.UseMiddleware<PluginMiddleware>();

application.MapGet("/", () => "Hello World!");

application.Run();