using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PampaGanadero.Core.Entities;
using PampaGanadero.Core.Enums;
using PampaGanadero.Core.Interfaces;

namespace PampaGanadero.Core.Services;

public class TagValidationService
{
    private readonly ISenasaLocalDb _db;

    public TagValidationService(ISenasaLocalDb db)
    {
        _db = db;
    }

    public async Task<TagValidationReport> ValidateAsync(EarTag tag, CancellationToken ct = default)
    {
        var report = new TagValidationReport { Tag = tag, Timestamp = DateTime.Now };

        if (!tag.SenasaNumber.IsValidForProvince("LP"))
            report.AddFinding("LP-001", "Caravana no corresponde a La Pampa.", Severity.High);

        var registered = await _db.FindBySenasaIdAsync(tag.SenasaNumber.Value, ct);
        if (registered == null)
        {
            report.AddFinding("SEN-002", "Caravana no registrada.", Severity.Critical);
            tag.UpdateStatus(TagStatus.NotRegistered);
        }
        else
        {
            var duplicates = await _db.GetDuplicateCountAsync(tag.Uid.Hex, ct);
            if (duplicates > 1)
                report.AddFinding("SEN-003", $"Caravana duplicada ({duplicates} veces).", Severity.High);
        }

        if (tag.Uid.Hex.StartsWith("E280")) // patrón batería baja en tags comunes
            tag.UpdateStatus(TagStatus.LowBattery);
        else
            tag.UpdateStatus(TagStatus.Active);

        report.Outcome = report.Findings.Count == 0 ? ValidationOutcome.Approved :
                         report.Findings.Exists(f => f.Severity == Severity.Critical) ? ValidationOutcome.Rejected :
                         ValidationOutcome.Warning;

        return report;
    }
}

public record TagValidationReport
{
    public EarTag Tag { get; init; } = null!;
    public DateTime Timestamp { get; init; }
    public List<Finding> Findings { get; } = new();
    public ValidationOutcome Outcome { get; set; }

    public void AddFinding(string code, string description, Severity severity = Severity.Medium)
        => Findings.Add(new Finding(code, description, severity));
}

public record Finding(string Code, string Description, Severity Severity);
public enum Severity { Low, Medium, High, Critical }
