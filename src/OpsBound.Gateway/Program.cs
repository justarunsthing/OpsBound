using Microsoft.EntityFrameworkCore;
using OpsBound.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpsBoundPersistence(builder.Configuration);
builder.Services.AddHealthChecks()
    .AddDbContextCheck<OpsBoundDbContext>();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OpsBoundDbContext>();
    await db.Database.MigrateAsync();
}

app.MapGet("/", () => Results.Ok(new { service = "OpsBound.Gateway" }));
app.MapHealthChecks("/health");

app.Run();
