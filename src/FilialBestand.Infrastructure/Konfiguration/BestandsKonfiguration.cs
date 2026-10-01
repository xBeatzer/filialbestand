using FilialBestand.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilialBestand.Infrastructure.Konfiguration;

public sealed class BestandKonfiguration : IEntityTypeConfiguration<Bestand>
{
    public void Configure(EntityTypeBuilder<Bestand> b)
    {
        b.HasIndex(x => new { x.FilialeId, x.ArtikelId }).IsUnique();

        b.HasOne(x => x.Filiale).WithMany(f => f.Bestaende).HasForeignKey(x => x.FilialeId);
        b.HasOne(x => x.Artikel).WithMany(a => a.Bestaende).HasForeignKey(x => x.ArtikelId);

        // Die Datenbank schützt sich selbst, auch wenn jemand am API vorbei schreibt
        b.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Bestand_Menge", "\"Menge\" >= 0");
            t.HasCheckConstraint("CK_Bestand_Ziel", "\"Zielbestand\" >= \"Mindestbestand\"");
        });
    }
}
