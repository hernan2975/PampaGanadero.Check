using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PampaGanadero.Core.Entities;
using PampaGanadero.Core.Interfaces;

namespace PampaGanadero.Infrastructure.Readers;

public class MockUHFReader : ITagReader
{
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task<IList<EarTag>> ScanAsync(TimeSpan duration)
    {
        await Task.Delay(500);
        return new List<EarTag>
        {
            EarTag.Create("E280112233445566778899AA", "LP0025481937", DateTime.Now.AddYears(-2), "INTA Anguil"),
            EarTag.Create("E200AABBCCDDEEFF11223344", "LP0025481938", DateTime.Now.AddYears(-1), "Min. Prod. LP")
        };
    }
}
