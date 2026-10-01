using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using FilialBestand.Domain;
using FilialBestand.Infrastructure;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace FilialBestand.Api.Controllers;

public sealed class ArtikelController(AppDbContext db) : ODataController
{
    // GET /odata/Artikel
    [EnableQuery(PageSize = 50)]
    public IQueryable<Artikel> Get() => db.Artikel.AsNoTracking();

    // GET /odata/Artikel(5)
    [EnableQuery]
    public SingleResult<Artikel> Get([FromRoute] int key)
        => SingleResult.Create(db.Artikel.AsNoTracking().Where(a => a.Id == key));
}