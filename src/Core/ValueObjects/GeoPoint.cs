namespace PampaGanadero.Core.ValueObjects;

public record GeoPoint(double Latitude, double Longitude)
{
    public bool IsInPampa() =>
        Latitude is >= -39.5 and <= -35.5 &&
        Longitude is >= -68.5 and <= -63.5;
}
