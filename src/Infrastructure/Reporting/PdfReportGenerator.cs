using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using PampaGanadero.Core.Services;

namespace PampaGanadero.Infrastructure.Reporting;

public class PdfReportGenerator : IReportGenerator
{
    public void Generate(TagValidationReport report, string outputPath)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);

        using var writer = new PdfWriter(outputPath);
        using var pdf = new PdfDocument(writer);
        var doc = new Document(pdf);

        doc.Add(new Paragraph("REPORTE DE VALIDACIÓN - PAMPA GANADERO").SetBold().SetFontSize(16));
        doc.Add(new Paragraph($"Caravana: {report.Tag.SenasaNumber.Value}"));
        doc.Add(new Paragraph($"UID: {report.Tag.Uid.Hex}"));
        doc.Add(new Paragraph($"Estado: {report.Outcome}"));
        doc.Add(new Paragraph($"Fecha: {report.Timestamp:dd/MM/yyyy HH:mm}"));
        doc.Add(new Paragraph(" "));

        if (report.Findings.Count > 0)
        {
            doc.Add(new Paragraph("HALLAZGOS:").SetBold());
            foreach (var f in report.Findings)
            {
                var prefix = f.Severity switch
                {
                    Severity.Critical => "❌ ",
                    Severity.High => "⚠️ ",
                    _ => "ℹ️ "
                };
                doc.Add(new Paragraph($"{prefix}{f.Code} - {f.Description}"));
            }
        }

        doc.Add(new Paragraph(" "));
        doc.Add(new Paragraph("Ministerio de la Producción - La Pampa").SetFontSize(10));
        doc.Close();
    }
}

public interface IReportGenerator
{
    void Generate(TagValidationReport report, string outputPath);
}
