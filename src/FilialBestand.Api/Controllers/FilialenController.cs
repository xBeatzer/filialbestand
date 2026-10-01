using FilialBestand.Domain;
using FilialBestand.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace FilialBestand.Api.Controllers;

public sealed class FilialenController(AppDbContext db, TimeProvider Zeit) : ODataController
{
    [EnableQuery]
    public IQueryable<Filiale> Get() => db.Filialen.AsNoTracking();

    [EnableQuery]
    public SingleResult<Filiale> Get([FromRoute] int key)
        => SingleResult.Create(db.Filialen.AsNoTracking().Where(f => f.Id == key));

    // FUNCTION  GET /odata/Filialen(1)/Default.Unterschreitungen()
    [HttpGet]
    [EnableQuery]
    public IQueryable<Bestand> Unterschreitungen([FromRoute] int key)
        => db.Bestaende.AsNoTracking()
             .Where(b => b.FilialeId == key && b.Menge < b.Mindestbestand);

    // ACTION  POST /odata/Filialen(1)/Default.NachbestellvorschlaegeErzeugen
    [HttpPost]
    public async Task<IActionResult> NachbestellvorschlaegeErzeugen([FromRoute] int key, CancellationToken ct)
    {
        if (!await db.Filialen.AnyAsync(f => f.Id == key, ct))
            return NotFound();

        // Für diese Artikel gibt es schon einen offenen Vorschlag → nicht doppelt anlegen
        var schonOffen = new HashSet<int>(await db.Nachbestellvorschlaege
            .Where(v => v.FilialeId == key && v.Status == VorschlagStatus.Offen)
            .Select(v => v.ArtikelId)
            .ToListAsync(ct));

        var kritisch = await db.Bestaende
            .Where(b => b.FilialeId == key && b.Menge < b.Mindestbestand)
            .ToListAsync(ct);

        var neue = new List<Nachbestellvorschlag>();
        foreach (var b in kritisch)
        {
            if (schonOffen.Contains(b.ArtikelId)) continue;
            if (NachbestellRechner.BerechneMenge(b) is not int menge) continue;

            neue.Add(new Nachbestellvorschlag
            {
                FilialeId = key,
                ArtikelId = b.ArtikelId,
                Menge = menge,
                ErstelltAm = Zeit.GetUtcNow(),    // Npgsql verlangt UTC für timestamptz
                Status = VorschlagStatus.Offen
            });
        }

        db.Nachbestellvorschlaege.AddRange(neue);
        try
        {
            await db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException)
        {
            // Zwei gleichzeitige Aufrufe → der partielle Unique-Index aus Phase 1 schlägt zu
            return Conflict("Vorschläge wurden parallel erzeugt, bitte erneut abrufen.");
        }

        return Ok(neue);
    }
}