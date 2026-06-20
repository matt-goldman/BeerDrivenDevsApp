using BeerDrivenDevsApp.Services;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Plugin.Maui.SmartNavigation.Attributes;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace BeerDrivenDevsApp;

[UseAutoDependencies]
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("ArialBold.ttf", "OpenSansSemibold");
                fonts.AddFont("lucide.ttf", "Lucide");
            })
            .UseSkiaSharp()
            .UseAutodependencies();

        builder.Services.AddHttpClient<IFileDownloadService, FileDownloadService>();
        builder.Services.AddHttpClient<IEpisodeService, EpisodeService>(opt => opt.BaseAddress = new Uri("https://www.beerdriven.dev"));


#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}