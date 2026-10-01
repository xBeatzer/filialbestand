using FilialBestand.Api;
using FilialBestand.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OData;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>((sp, o) =>
{
    // Connection-String zur Laufzeit aus der Konfiguration lesen.
    var cs = sp.GetRequiredService<IConfiguration>().GetConnectionString("Default");
    o.UseNpgsql(cs);

    o.UseSeeding((ctx, _) => DemoDaten.FilialenAnlegen(ctx));
    o.UseAsyncSeeding((ctx, _, ct) => DemoDaten.FilialenAnlegenAsync(ctx, ct));
});
builder.Services.AddControllers().AddOData(o => o
    .Select().Filter().OrderBy().Expand().Count()
    .SetMaxTop(100)                                   // niemand darf mehr als 100 Datensätze auf einmal holen
    .AddRouteComponents("odata", EdmModell.Erstellen()));

builder.Services.AddSingleton(TimeProvider.System);

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
