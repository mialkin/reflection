﻿using Microsoft.AspNetCore.Http;
using RawCoding.EndpointPdk;

namespace RawCoding.TestEndpoint;

[Path("get", "/plugin/test")]
public class AnEndpoint : IPluginEndpoint
{
    public async Task ExecuteAsync(HttpContext context)
    {
        await context.Response.WriteAsync("Test");
        
        // var jsonString = JsonSerializer.Serialize(new { Message = "Hello!" });
        // await context.Response.WriteAsJsonAsync(new { Message = "Hello! 1234" });
    }
}