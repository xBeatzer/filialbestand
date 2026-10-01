using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using FilialBestand.Domain;

namespace FilialBestand.Api;

public static class EdmModell
{
    public static IEdmModel Erstellen()
    {
        var b = new ODataConventionModelBuilder();

        b.EntitySet<Artikel>("Artikel");
        b.EntitySet<Filiale>("Filialen");
        b.EntitySet<Bestand>("Bestaende");
        b.EntitySet<Nachbestellvorschlag>("Nachbestellvorschlaege");

        // FUNCTION: nur lesen, keine Seiteneffekte → per GET aufrufbar
        b.EntityType<Filiale>()
            .Function("Unterschreitungen")
            .ReturnsCollectionFromEntitySet<Bestand>("Bestaende");

        // ACTION: verändert Daten → nur per POST aufrufbar
        b.EntityType<Filiale>()
            .Action("NachbestellvorschlaegeErzeugen")
            .ReturnsCollectionFromEntitySet<Nachbestellvorschlag>("Nachbestellvorschlaege");

        return b.GetEdmModel();
    }
}