using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using PampaGanadero.Core.Entities;
using PampaGanadero.Core.Interfaces;
using PampaGanadero.Core.ValueObjects;

namespace PampaGanadero.Infrastructure.Data;

public class LocalSenasaDb : ISenasaLocalDb
{
    private readonly string _dbPath = "data/senasa_lp.db";

    public LocalSenasaDb()
    {
        EnsureDb();
    }

    private void EnsureDb()
    {
        if (!System.IO.Directory.Exists("data"))
            System.IO.Directory.CreateDirectory("data");

        using var conn = new SqliteConnection($"Data Source={_dbPath}");
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS tags (
                uid TEXT PRIMARY KEY,
                senasa_id TEXT NOT NULL,
                issue_date TEXT NOT NULL,
                entity TEXT NOT NULL
            )";
        cmd.ExecuteNonQuery();
    }

    public async Task<EarTag?> FindBySenasaIdAsync(string senasaId, CancellationToken ct = default)
    {
        await using var conn = new SqliteConnection($"Data Source={_dbPath}");
        await conn.OpenAsync(ct);
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT uid, senasa_id, issue_date, entity FROM tags WHERE senasa_id = $id";
        cmd.Parameters.AddWithValue("$id", senasaId);

        await using var reader = await cmd.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct))
        {
            return EarTag.Create(
                reader.GetString(0),
                reader.GetString(1),
                DateTime.Parse(reader.GetString(2)),
                reader.GetString(3)
            );
        }
        return null;
    }

    public async Task<int> GetDuplicateCountAsync(string uidHex, CancellationToken ct = default)
    {
        await using var conn = new SqliteConnection($"Data Source={_dbPath}");
        await conn.OpenAsync(ct);
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT COUNT(*) FROM tags WHERE uid = $uid";
        cmd.Parameters.AddWithValue("$uid", uidHex);

        var result = await cmd.ExecuteScalarAsync(ct);
        return Convert.ToInt32(result);
    }
}
