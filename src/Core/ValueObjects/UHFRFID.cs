namespace PampaGanadero.Core.ValueObjects;

public record UHFRFID(string Hex)
{
    public static UHFRFID FromHex(string hex)
    {
        if (string.IsNullOrWhiteSpace(hex)) throw new ArgumentException("UID hex required.");
        var clean = hex.Replace("-", "").Replace(":", "").ToUpper();
        if (clean.Length != 24) throw new ArgumentException("Invalid UHF RFID length (expected 24 chars).");
        return new UHFRFID(clean);
    }
}
