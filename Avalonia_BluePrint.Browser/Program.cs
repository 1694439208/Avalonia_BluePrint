using Avalonia;
using Avalonia.Browser;
using Avalonia.Media;
using Avalonia.Media.Fonts;
using Avalonia.ReactiveUI;
using Avalonia_BluePrint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;

[assembly: SupportedOSPlatform("browser")]

internal partial class Program
{
    internal static readonly IReadOnlyDictionary<string, string> fontOverrides = new Dictionary<string, string>()
    {
        // the avalonia way of specifying embedded fonts
        ["zh"] = "fonts:Noto Sans SC#Noto Sans SC",
        ["en"] = "fonts:Noto Sans#Noto Sans",
        ["es"] = "fonts:Noto Sans#Noto Sans",
        ["pt"] = "fonts:Noto Sans#Noto Sans",
        ["fr"] = "fonts:Noto Sans#Noto Sans",
        ["ru"] = "fonts:Noto Sans#Noto Sans",
    };

    private static void SetCultureSpecificFontOptions(AppBuilder builder, string culture, string fontFamily)
    {
        if (Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == culture)
        {
            FamilyNameCollection families = new(fontFamily);
            _ = builder.With(new FontManagerOptions()
            {
                DefaultFamilyName = families.PrimaryFamilyName,
                FontFallbacks = families
                    .Skip(1)
                    .Select(name => new FontFallback()
                    {
                        FontFamily = name
                    })
                    .ToList()
            });
        }
    }

    private static async Task Main(string[] args) => await BuildAvaloniaApp()
            .UseReactiveUI()
            .StartBrowserAppAsync("out");

    public static AppBuilder BuildAvaloniaApp()
    {
        AppBuilder builder = AppBuilder
            .Configure<App>()
            .ConfigureFonts(manager =>
            {
                manager.AddFontCollection(new EmbeddedFontCollection(
                        new Uri("fonts:Noto Sans SC", UriKind.Absolute),
                        new Uri("avares://Avalonia_BluePrint/Assets/Fonts/NotoSansSC", UriKind.Absolute)));
                manager.AddFontCollection(new EmbeddedFontCollection(
                    new Uri("fonts:Noto Sans", UriKind.Absolute),
                    new Uri("avares://Avalonia_BluePrint/Assets/Fonts/NotoSans", UriKind.Absolute)));
            });

        foreach ((string culture, string fontFamily) in fontOverrides)
        {
            SetCultureSpecificFontOptions(builder, culture, fontFamily);
        }

        return builder;
    }
}