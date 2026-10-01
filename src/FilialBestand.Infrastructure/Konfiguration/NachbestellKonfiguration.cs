using FilialBestand.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilialBestand.Infrastructure.Konfiguration;

public sealed class NachbestellvorschlagKonfiguration : IEntityTypeConfiguration<Nachbestellvorschlag>
{
    public void Configure(EntityTypeBuilder<Nachbestellvorschlag> b)
    {
        b.Property(x => x.Status).HasConversion<string>().HasMaxLength(20); // lesbar in der DB

        // Pro Filiale und Artikel darf es höchstens EINEN offenen Vorschlag geben.
        b.HasIndex(x => new { x.FilialeId, x.ArtikelId })
            .IsUnique()
            .HasFilter("\"Status\" = 'Offen'");
    }
}
