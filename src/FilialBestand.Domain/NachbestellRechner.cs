namespace FilialBestand.Domain;

public static class NachbestellRechner
{
    /// <summary>
    /// Liefert die Nachbestellmenge oder null, wenn nichts zu bestellen ist.
    /// Regel: Liegt die Menge unter dem Mindestbestand, wird bis zum Zielbestand aufgefüllt.
    /// </summary>
    public static int? BerechneMenge(Bestand b)
    {
        if (b.Zielbestand < b.Mindestbestand)
            throw new ArgumentException("Zielbestand darf nicht unter dem Mindestbestand liegen.", nameof(b));

        if (b.Menge >= b.Mindestbestand)
            return null;

        return b.Zielbestand - b.Menge;
    }
}
