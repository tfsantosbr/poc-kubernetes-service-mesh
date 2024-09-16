var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapGet("/version", (IConfiguration configuration) =>
{
    return Results.Ok(configuration["AppVersion"]);
});

app.MapHealthChecks("/health");

app.Run();