
using Khandon.SharerdKernel.UI;
using Microsoft.AspNetCore.Components.WebView.Maui;

namespace Khandon.Persentation.MAUI
{
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

            builder.Services.AddBlazorWebView();

            builder.Services.AddUI();
            return builder.Build();
        }
    }
}