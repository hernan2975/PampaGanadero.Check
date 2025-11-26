using PampaGanadero.Core.Services;

namespace PampaGanadero.Core.Interfaces;

public interface IReportGenerator
{
    void Generate(TagValidationReport report, string outputPath);
}
