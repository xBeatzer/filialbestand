using FilialBestand.Domain;
using Microsoft.EntityFrameworkCore;

namespace FilialBestand.Infrastructure;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Artikel> Artikel => Set<Artikel>();
    public DbSet<Filiale> Filialen => Set<Filiale>();
    public DbSet<Bestand> Bestaende => Set<Bestand>();
    public DbSet<Nachbestellvorschlag> Nachbestellvorschlaege => Set<Nachbestellvorschlag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
}
