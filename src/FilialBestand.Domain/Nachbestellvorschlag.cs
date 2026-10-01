namespace FilialBestand.Domain;

public class Nachbestellvorschlag
{
    public int Id { get; set; }
    public int FilialeId { get; set; }
    public Filiale? Filiale { get; set; }
    public int ArtikelId { get; set; }
    public Artikel? Artikel { get; set; }
    public int Menge { get; set; }
    public DateTimeOffset ErstelltAm { get; set; }
    public VorschlagStatus Status { get; set; }
}