using System;
using System.IO;
using System.Threading.Tasks;
using PampaGanadero.Infrastructure.Data;

class SenasaDbUpdater
{
    static async Task<int> Main(string[] args)
    {
        Console.WriteLine("🔧 Actualizador de Padrón SENASA — La Pampa");
        Console.WriteLine("Modo: USB (por defecto) | --api para descarga remota");
        Console.WriteLine();

        var sync = new SenasaSyncService();

        if (args.Length > 0 && args[0] == "--api")
        {
            Console.Write("URL API SENASA: ");
            var url = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(url))
            {
                Console.WriteLine("❌ URL requerida.");
                return 1;
            }

            Console.WriteLine("Descargando...");
            var success = await sync.SyncFromApiAsync(url);
            Console.WriteLine(success ? "✅ Actualización completada." : "❌ Falló la descarga.");
            return success ? 0 : 1;
        }
        else
        {
            Console.Write("Ruta USB (ej: D:\\): ");
            var path = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
            {
                Console.WriteLine("❌ Ruta no válida.");
                return 1;
            }

            var success = sync.SyncFromUsb(path);
            Console.WriteLine(success ? "✅ Base de datos actualizada desde USB." : "❌ No se encontró senasa_lp.db en la unidad.");
            return success ? 0 : 1;
        }
    }
}
