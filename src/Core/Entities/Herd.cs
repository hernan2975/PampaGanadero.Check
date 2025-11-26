using System.Collections.Generic;

namespace PampaGanadero.Core.Entities;

public class Herd
{
    public string Name { get; set; } = string.Empty;
    public string Owner { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public List<Animal> Animals { get; set; } = new();
}
