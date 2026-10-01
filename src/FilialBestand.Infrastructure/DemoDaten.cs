using FilialBestand.Domain;
using Microsoft.EntityFrameworkCore;

namespace FilialBestand.Infrastructure;

public static class DemoDaten
{
	private static Filiale[] Filialen() =>
	[
		new() { Nummer = "F001", Name = "Dortmund City",   Ort = "Dortmund" },
		new() { Nummer = "F002", Name = "Dortmund Hörde",  Ort = "Dortmund" },
		new() { Nummer = "F003", Name = "Unna Zentrum",    Ort = "Unna" },
	];

	public static void FilialenAnlegen(DbContext ctx)
	{
		if (ctx.Set<Filiale>().Any()) return;          // idempotent!
		ctx.Set<Filiale>().AddRange(Filialen());
		ctx.SaveChanges();
	}

	public static async Task FilialenAnlegenAsync(DbContext ctx, CancellationToken ct)
	{
		if (await ctx.Set<Filiale>().AnyAsync(ct)) return;
		ctx.Set<Filiale>().AddRange(Filialen());
		await ctx.SaveChangesAsync(ct);
	}
}