using Microsoft.AspNetCore.Mvc;
using FilialBestand.Domain;
using FilialBestand.Infrastructure;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OData.Deltas;

namespace FilialBestand.Api.Controllers;

public sealed class BestaendeController(AppDbContext db) : ODataController
{
    [EnableQuery(PageSize = 50)]
    public IQueryable<Bestand> Get() => db.Bestaende.AsNoTracking();

    public async Task<IActionResult> Post([FromBody] Bestand neu, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);   // [Range]-Attribute greifen hier
        if (neu.Zielbestand < neu.Mindestbestand)
            return BadRequest("Zielbestand muss >= Mindestbestand sein.");

        db.Bestaende.Add(neu);
        try
        {
            await db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException)
        {
            // Unique-Index (FilialeId, ArtikelId) verletzt → fachlich ein Konflikt
            return Conflict("Für diese Filiale und diesen Artikel existiert bereits ein Bestand.");
        }
        return Created(neu);                                       // 201 + Location-Header
    }

    // PATCH /odata/Bestaende(3)   Body: { "Menge": 4 }
    public async Task<IActionResult> Patch([FromRoute] int key, [FromBody] Delta<Bestand> delta, CancellationToken ct)
    {
        var bestand = await db.Bestaende.FindAsync([key], ct);
        if (bestand is null) return NotFound();

        delta.Patch(bestand);                     // übernimmt NUR die Felder, die im Body standen
        if (bestand.Menge < 0 || bestand.Zielbestand < bestand.Mindestbestand)
            return BadRequest("Ungültige Werte.");

        await db.SaveChangesAsync(ct);
        return Updated(bestand);
    }
}