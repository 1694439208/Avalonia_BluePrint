using Avalonia;
using Avalonia.Browser;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using Avalonia_BluePrint;
using System.Runtime.Versioning;
using System.Threading.Tasks;

[assembly: SupportedOSPlatform("browser")]

internal partial class Program
{
    private static async Task Main(string[] args) => await BuildAvaloniaApp()
            .WithInterFont()
            .UseReactiveUI()
            .StartBrowserAppAsync("out");

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
        .With(new FontManagerOptions()
        {
            FontFallbacks = new[]
                {
                    new FontFallback
                    {
                        FontFamily = new FontFamily("avares://Avalonia_BluePrint/Assets/Fonts/AlibabaPuHuiTi-2-45-Light.ttf")
                    }
                }
        });
}