using Microsoft.Extensions.Logging;
using Microsoft.Maui.Hosting;
using PampaGanadero.Core.Interfaces;
using PampaGanadero.Infrastructure.Data;
using PampaGanadero.Infrastructure.Readers;

namespace PampaGanadero.Presentation.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Registros de dependencias
        builder.Services.AddSingleton<ITagReader, MockUHFReader>();
        builder.Services.AddSingleton<ISenasaLocalDb, LocalSenasaDb>();
        builder.Services.AddTransient<MainPage>();

        return builder.Build();
    }
}
