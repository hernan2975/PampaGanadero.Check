using System.Collections.Generic;
using System.Threading.Tasks;
using PampaGanadero.Core.Entities;

namespace PampaGanadero.Core.Interfaces;

public interface ITagReader
{
    Task InitializeAsync();
    Task<IList<EarTag>> ScanAsync(TimeSpan duration);
}
