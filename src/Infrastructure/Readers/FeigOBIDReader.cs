using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PampaGanadero.Core.Entities;
using PampaGanadero.Core.Interfaces;

namespace PampaGanadero.Infrastructure.Readers;

public class FeigOBIDReader : ITagReader
{
    private readonly string _comPort;
    private bool _initialized;

    public FeigOBIDReader(string comPort = "COM3")
    {
        _comPort = comPort;
    }

    public async Task InitializeAsync()
    {
        // Simulación de inicialización de hardware Feig OBID
        await Task.Delay(300);
        _initialized = true;
    }

    public async Task<IList<EarTag>> ScanAsync(TimeSpan duration)
    {
        if (!_initialized)
            throw new InvalidOperationException("Reader not initialized.");

        // Simulación de lectura real: en producción usar SDK Feig o comandos serie
        await Task.Delay((int)duration.TotalMilliseconds / 2);

        return new List<EarTag>
        {
            EarTag.Create("E28011002233445566778899", "LP0088776655", DateTime.Now.AddYears(-1), "Min. Prod. LP")
        };
    }
}
