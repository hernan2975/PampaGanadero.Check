using System.Threading;
using System.Threading.Tasks;
using PampaGanadero.Core.Entities;

namespace PampaGanadero.Core.Interfaces;

public interface ISenasaLocalDb
{
    Task<EarTag?> FindBySenasaIdAsync(string senasaId, CancellationToken ct = default);
    Task<int> GetDuplicateCountAsync(string uidHex, CancellationToken ct = default);
}
