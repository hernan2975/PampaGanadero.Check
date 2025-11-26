using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading.Tasks;
using PampaGanadero.Core.Entities;
using PampaGanadero.Core.Interfaces;

namespace PampaGanadero.Infrastructure.Readers;

public class SerialPortUHFAdapter : ITagReader
{
    private readonly string _portName;
    private SerialPort? _port;

    public SerialPortUHFAdapter(string portName = "COM3")
    {
        _portName = portName;
    }

    public async Task InitializeAsync()
    {
        _port = new SerialPort(_portName, 115200, Parity.None, 8, StopBits.One)
        {
            ReadTimeout = 1000,
            WriteTimeout = 1000
        };

        try
        {
            _port.Open();
            // Enviar comando de inicialización genérico (ej: "AT+SCAN=1")
            await _port.WriteLineAsync("AT+INIT\r\n");
            await Task.Delay(200);
        }
        catch (Exception)
        {
            _port?.Dispose();
            _port = null;
            throw new InvalidOperationException($"No se pudo abrir el puerto {_portName}.");
        }
    }

    public async Task<IList<EarTag>> ScanAsync(TimeSpan duration)
    {
        if (_port == null || !_port.IsOpen)
            throw new InvalidOperationException("Port not initialized.");

        var tags = new List<EarTag>();
        var endTime = DateTime.Now + duration;

        // Simulación: en producción leer buffer serie y parsear respuestas UHF
        while (DateTime.Now < endTime)
        {
            await Task.Delay(100);
            // Ejemplo de respuesta real: "+SCAN:E280112233445566778899AA,LP0025481937"
            // Aquí iría el parser real
        }

        return tags;
    }

    public void Dispose() => _port?.Dispose();
}
