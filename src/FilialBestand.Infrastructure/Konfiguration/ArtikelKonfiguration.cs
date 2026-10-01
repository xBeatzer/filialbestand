using FilialBestand.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilialBestand.Infrastructure.Konfiguration;

public sealed class ArtikelKonfiguration : IEntityTypeConfiguration<Artikel>
{
    public void Configure(EntityTypeBuilder<Artikel> b)
    {
        b.HasIndex(a => a.SapProduktNummer).IsUnique();
    }
}