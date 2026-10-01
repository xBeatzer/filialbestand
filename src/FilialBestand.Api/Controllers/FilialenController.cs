using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using FilialBestand.Infrastructure;
using FilialBestand.Domain;

namespace FilialBestand.Api.Controllers;

public sealed class FilialenController(AppDbContext db, TimeProvider Zeit) : ODataController
{

}