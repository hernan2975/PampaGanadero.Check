using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PampaGanadero.Core.Entities;
using PampaGanadero.Core.Interfaces;
using PampaGanadero.Core.Services;
using PampaGanadero.Infrastructure.Data;
using PampaGanadero.Infrastructure.Readers;
using PampaGanadero.Infrastructure.Reporting;

class Program
{
    static async Task Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddSingleton<ITagReader>(sp =>
            args.Contains("--feig") ? (ITagReader)new FeigOBIDReader() :
            args.Contains("--serial") ? new SerialPortUHFAdapter(args.Contains("--port") ? args[Array.IndexOf(args, "--port") + 1] : "COM3") :
            new MockUHFReader());
        services.AddSingleton<ISenasaLocalDb, LocalSenasaDb>();
        services.AddSingleton<TagValidationService>();
        services.AddSingleton<IReportGenerator, PdfReportGenerator>();

        var provider = services.BuildServiceProvider();
        var reader = provider.GetRequiredService<ITagReader>();
        var validator = provider.GetRequiredService<TagValidationService>();
        var reporter = provider.GetRequiredService<IReportGenerator>();

        await reader.InitializeAsync();

        Console.Clear();
        Console.WriteLine("🐄 PampaGanadero.Check v1.0 — La Pampa");
        Console.WriteLine("Validador de caravanas *offline-first* para campo");
        Console.WriteLine("------------------------------------------------");
        Console.WriteLine("Comandos:");
        Console.WriteLine("  [E] Escanear caravana");
        Console.WriteLine("  [B] Modo batch (5 lecturas)");
        Console.WriteLine("  [Q] Salir");
        Console.WriteLine();
        Console.WriteLine($"📡 Lector: {reader.GetType().Name}");
        Console.WriteLine($"🗃️  Base SENASA: {(File.Exists("data/senasa_lp.db") ? "Cargada" : "No encontrada — usar tools/senasa-db-sync")}");
        Console.WriteLine();

        while (true)
        {
            Console.Write("► ");
            var input = Console.ReadKey().Key;
            Console.WriteLine();

            try
            {
                if (input == ConsoleKey.E)
                {
                    await ProcessSingleScan(reader, validator, reporter);
                }
                else if (input == ConsoleKey.B)
                {
                    await ProcessBatchScan(reader, validator, reporter);
                }
                else if (input == ConsoleKey.Q)
                {
                    break;
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Error: {ex.Message}");
                Console.ResetColor();
            }
        }
    }

    static async Task ProcessSingleScan(ITagReader reader, TagValidationService validator, IReportGenerator reporter)
    {
        Console.WriteLine("🔍 Escaneando... (3 segundos)");
        var tags = await reader.ScanAsync(TimeSpan.FromSeconds(3));

        if (tags.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("⚠️  No se detectaron caravanas.");
            Console.ResetColor();
            return;
        }

        var tag = tags[0];
        var report = await validator.ValidateAsync(tag);

        Console.WriteLine();
        Console.WriteLine($"🆔 {tag.SenasaNumber.Value}");
        Console.WriteLine($"📡 UID: {tag.Uid.Hex}");
        Console.WriteLine($"📍 Origen: {tag.IssuingEntity}");

        switch (report.Outcome)
        {
            case Core.Enums.ValidationOutcome.Approved:
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("✅ ACEPTADA");
                break;
            case Core.Enums.ValidationOutcome.Warning:
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("⚠️  CONDICIONAL — Revisar");
                break;
            case Core.Enums.ValidationOutcome.Rejected:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ RECHAZADA");
                break;
        }
        Console.ResetColor();

        foreach (var f in report.Findings)
        {
            var icon = f.Severity == Core.Enums.Severity.Critical ? "❌" : f.Severity == Core.Enums.Severity.High ? "⚠️ " : "ℹ️ ";
            Console.WriteLine($"   {icon} {f.Code}: {f.Description}");
        }

        if (report.Outcome != Core.Enums.ValidationOutcome.Approved)
        {
            Directory.CreateDirectory("reports");
            var path = $"reports/alert_{tag.SenasaNumber.Value}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            reporter.Generate(report, path);
            Console.WriteLine($"📄 Reporte generado: {Path.GetFileName(path)}");
        }

        Console.WriteLine();
    }

    static async Task ProcessBatchScan(ITagReader reader, TagValidationService validator, IReportGenerator reporter)
    {
        Console.WriteLine("🔁 Modo batch: 5 escaneos en 10 segundos");
        var results = new System.Collections.Generic.List<(string Id, Core.Enums.ValidationOutcome Outcome)>();

        for (int i = 0; i < 5; i++)
        {
            var tags = await reader.ScanAsync(TimeSpan.FromSeconds(2));
            if (tags.Count > 0)
            {
                var report = await validator.ValidateAsync(tags[0]);
                results.Add((tags[0].SenasaNumber.Value, report.Outcome));
                Console.Write($"✓");
            }
            else
            {
                results.Add(("—", Core.Enums.ValidationOutcome.Rejected));
                Console.Write("✗");
            }
            await Task.Delay(500);
        }

        Console.WriteLine();
        Console.WriteLine("\n📊 Resultados:");
        foreach (var (id, outcome) in results)
        {
            var mark = outcome switch
            {
                Core.Enums.ValidationOutcome.Approved => "✅",
                Core.Enums.ValidationOutcome.Warning => "⚠️ ",
                _ => "❌"
            };
            Console.WriteLine($" {mark} {id}");
        }
    }
}
