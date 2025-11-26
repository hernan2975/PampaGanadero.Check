using PampaGanadero.Core.ValueObjects;

namespace PampaGanadero.Core.Entities;

public class Animal
{
    public string SenasaId { get; set; } = string.Empty;
    public string Breed { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public GeoPoint? LastKnownLocation { get; set; }
    public EarTag? PrimaryTag { get; set; }
    public EarTag? SecondaryTag { get; set; }
}
