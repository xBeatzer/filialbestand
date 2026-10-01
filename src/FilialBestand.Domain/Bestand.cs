using System.ComponentModel.DataAnnotations;

namespace FilialBestand.Domain;

public class Bestand
{
    public int Id { get; set; }                 // eigener Schlüssel statt zusammengesetztem (siehe Kasten)
    public int FilialeId { get; set; }
    public Filiale? Filiale { get; set; }
    public int ArtikelId { get; set; }
    public Artikel? Artikel { get; set; }

    [Range(0, int.MaxValue)] public int Menge { get; set; }
    [Range(0, int.MaxValue)] public int Mindestbestand { get; set; }
    [Range(0, int.MaxValue)] public int Zielbestand { get; set; }
}