using Microsoft.Extensions.DependencyInjection;
using PampaGanadero.Core.Interfaces;
using PampaGanadero.Core.Services;
using PampaGanadero.Infrastructure.Data;
using PampaGanadero.Infrastructure.Readers;
using PampaGanadero.Infrastructure.Reporting;

var services = new ServiceCollection();
services.AddSingleton<ITagReader, MockUHFReader>();
services.AddSingleton<ISenasaLocalDb, LocalSenasaDb>();
services.AddSingleton<TagValidationService>();
services.AddSingleton<IReportGenerator, PdfReportGenerator>();

var provider = services.BuildServiceProvider();
var reader = provider.GetRequiredService<ITagReader>();
var validator = provider.GetRequiredService<TagValidationService>();
var reporter = provider.GetRequiredService<IReportGenerator>();

await reader.InitializeAsync();

Console.WriteLine("🔍 PampaGanadero.Check — Validador de Caravanas (La Pampa)");
Console.WriteLine("Modo: Simulado — Presione [E] para escanear");

while (true)
{
    var key = Console.ReadKey().Key;
    if (key == ConsoleKey.E)
    {
        Console.WriteLine("\nEscaneando...");
        var tags = await reader.ScanAsync(TimeSpan.FromSeconds(3));

        foreach (var tag in tags)
        {
            var report = await validator.ValidateAsync(tag);
            Console.WriteLine($"\n✅ {tag.SenasaNumber.Value} → {report.Outcome}");
            foreach (var f in report.Findings)
                Console.WriteLine($"   {(f.Severity == Severity.Critical ? "❌" : "⚠️")} {f.Code}: {f.Description}");

            if (report.Outcome != ValidationOutcome.Approved)
            {
                var path = $"reports/alert_{tag.SenasaNumber.Value}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                reporter.Generate(report, path);
                Console.WriteLine($"📄 Reporte: {path}");
            }
        }
    }
    else if (key == ConsoleKey.Q)
        break;
}
