using FilialBestand.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>((sp, o) =>
{
    // Connection-String zur Laufzeit aus der Konfiguration lesen.
    // Wichtig für die Tests in Phase 4: Dort wird er überschrieben.
    var cs = sp.GetRequiredService<IConfiguration>().GetConnectionString("Default");
    o.UseNpgsql(cs);

    o.UseSeeding((ctx, _) => DemoDaten.FilialenAnlegen(ctx));
    o.UseAsyncSeeding((ctx, _, ct) => DemoDaten.FilialenAnlegenAsync(ctx, ct));
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program;
