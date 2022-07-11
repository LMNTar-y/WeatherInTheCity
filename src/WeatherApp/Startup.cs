using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Polly;
using WeatherApp.Business;
using WeatherApp.Data.Models;
using WeatherApp.Data.Services;

namespace WeatherApp.Main;

public static class Startup
{
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        var hostBuilder = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, builder) =>
            {
                builder.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", false)
                    .AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<IFileStorageService, FileStorageService>()
                    .AddSingleton<IWeatherReceiverService, WeatherReceiverService>()
                    .AddSingleton<IReceiveAndSaveService, ReceiveAndSaveService>()
                    .AddLogging(loggingBuilder =>
                    {
                        // configure Logging with NLog
                        loggingBuilder.ClearProviders();
                        loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                        loggingBuilder.AddNLog();
                    });
                services.Configure<PathToFileConfig>(context.Configuration.GetSection(nameof(PathToFileConfig)));
                services.Configure<ConnectionSettings>(context.Configuration.GetSection(nameof(ConnectionSettings)));

                var jitterer = new Random();
                services.AddHttpClient("WeatherApp",
                        c =>
                        {
                            c.BaseAddress = new Uri(context.Configuration
                                [$"{nameof(ConnectionSettings)}:{nameof(ConnectionSettings.Url)}"]);
                        })
                    .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(3, retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) +
                        TimeSpan.FromMilliseconds(jitterer.Next(0, 1000))))
                    .AddTransientHttpErrorPolicy(builder => builder.CircuitBreakerAsync(
                        3,
                        TimeSpan.FromSeconds(30)
                    ));
            });

        return hostBuilder;
    }
}