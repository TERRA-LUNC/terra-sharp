using Binance.Net;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using TerraSharp.Maui.Example.Data;
using TerraSharp.Maui.Example.Pages;
using TerraSharp.Maui.Example.ViewModels;

namespace TerraSharp.Maui.Example
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            //Preferences.Default.Set("key", "WBFyro5t2yjcr6AqpLDfM4rQIXB3qKc60qjiEYGDhjyofB25lkbNogSnQBthMtxt");
            //Preferences.Default.Set("secret", "S6MJBO8aA5Uaf0TzP7kYJqCEQPYdmurXYaEsHW8651jE89ZlxB9o8QpadQPee6Z1");

            builder
                .UseSkiaSharp(true)
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Gotham-Light.ttf", "Gotham-Light");
                    fonts.AddFont("Gotham-Medium.ttf", "Gotham-Medium");
                    fonts.AddFont("Gotham-Bold.ttf", "Gotham-Bold");
                    fonts.AddFont("Gotham-Book.ttf", "Gotham-Book");
                    fonts.AddFont("FluentSystemIcons-Filled.ttf", "FluentFilled");
                    fonts.AddFont("FluentSystemIcons-Regular.ttf", "FluentRegular");
                });

            builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);

            builder.Services.AddBinance((restClientOptions, socketClientOptions) => {
                restClientOptions.ApiCredentials = new Binance.Net.Objects.BinanceApiCredentials(Preferences.Default.Get("key", ""), Preferences.Default.Get("secret", ""));
                
                restClientOptions.LogLevel = LogLevel.Trace;

                socketClientOptions.ApiCredentials = new Binance.Net.Objects.BinanceApiCredentials(Preferences.Default.Get("key", ""), Preferences.Default.Get("secret", ""));
            });

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();

            builder.Services.AddSingleton<WalletsDatabase>();
            return builder.Build();
        }
    }
}