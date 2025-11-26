namespace PampaGanadero.Core.ValueObjects;

public record SenasaId(string Value)
{
    public static SenasaId Parse(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("SENASA ID required.");
        var clean = value.Trim().ToUpper();
        if (!clean.StartsWith("LP")) throw new ArgumentException("SENASA ID must start with 'LP' for La Pampa.");
        if (clean.Length != 12) throw new ArgumentException("SENASA ID must be 12 chars.");
        return new SenasaId(clean);
    }

    public bool IsValidForProvince(string provinceCode) => Value.StartsWith(provinceCode.ToUpper());
}
