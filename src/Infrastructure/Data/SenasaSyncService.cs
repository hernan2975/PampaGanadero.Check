using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PampaGanadero.Infrastructure.Data;

public class SenasaSyncService
{
    private readonly HttpClient _httpClient = new();
    private readonly string _dbPath = "data/senasa_lp.db";

    public async Task<bool> SyncFromApiAsync(string apiUrl, CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.GetAsync(apiUrl, ct);
            if (!response.IsSuccessStatusCode) return false;

            var json = await response.Content.ReadAsStringAsync(ct);
            var updates = JsonConvert.DeserializeObject<List<SenasaRecord>>(json);

            // Actualizar base local (simplificado)
            File.WriteAllText("data/senasa_latest.json", json);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool SyncFromUsb(string usbPath)
    {
        var source = Path.Combine(usbPath, "senasa_lp.db");
        if (!File.Exists(source)) return false;

        Directory.CreateDirectory("data");
        File.Copy(source, _dbPath, overwrite: true);
        return true;
    }
}

public class SenasaRecord
{
    public string Uid { get; set; } = string.Empty;
    public string SenasaId { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public string Entity { get; set; } = string.Empty;
}
