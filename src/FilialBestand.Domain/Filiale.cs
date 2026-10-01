using System.ComponentModel.DataAnnotations;

namespace FilialBestand.Domain;

public class Filiale
{
    public int Id { get; set; }
    [MaxLength(10)] public string Nummer { get; set; } = "";
    [MaxLength(100)] public string Name { get; set; } = "";
    [MaxLength(100)] public string Ort { get; set; } = "";

    public List<Bestand> Bestaende { get; set; } = new();
}
