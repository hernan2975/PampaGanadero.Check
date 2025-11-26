using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

class FakeUHFTagger
{
    static void Main(string[] args)
    {
        Console.WriteLine("🐄 Generador de Caravanas Simuladas — La Pampa");
        Console.WriteLine("Uso: FakeUHFTagger <cantidad> [archivo.csv]");
        Console.WriteLine();

        if (args.Length == 0)
        {
            Console.WriteLine("Ejemplo: FakeUHFTagger 10 tags.csv");
            return;
        }

        if (!int.TryParse(args[0], out int count) || count <= 0)
        {
            Console.WriteLine("❌ Cantidad debe ser un número positivo.");
            return;
        }

        var outputFile = args.Length > 1 ? args[1] : "fake_tags.csv";
        var random = new Random();

        using var writer = new StreamWriter(outputFile);
        writer.WriteLine("uid,senasa_id,issue_date,entity");

        for (int i = 0; i < count; i++)
        {
            // Generar UID UHF válido (24 hex chars)
            var uid = "E2" + string.Concat(Enumerable.Range(0, 22).Select(_ => "0123456789ABCDEF"[random.Next(16)]));
            
            // Generar ID SENASA LP válido
            var senasa = "LP" + string.Format("{0:D10}", 2500000000L + random.Next(100000000));
            
            var date = DateTime.Now.AddYears(-random.Next(1, 5));
            var entity = new[] { "INTA Anguil", "Min. Prod. LP", "Coop. Eduardo Castex" }[random.Next(3)];

            writer.WriteLine($"{uid},{senasa},{date:yyyy-MM-dd},{entity}");
        }

        Console.WriteLine($"✅ Generadas {count} caravanas en {outputFile}");
    }
}
