using System.ComponentModel.DataAnnotations;

namespace FilialBestand.Domain;

public class Artikel
{
    public int Id { get; set; }
    [MaxLength(40)] public string SapProduktNummer { get; set; } = "";
    [MaxLength(200)] public string Bezeichnung { get; set; } = "";
    [MaxLength(3)] public string Basiseinheit { get; set; } = "";
    [MaxLength(20)] public string? Warengruppe { get; set; }
    public DateTimeOffset ZuletztSynchronisiert { get; set; }

    public List<Bestand> Bestaende { get; set; } = new();
}
