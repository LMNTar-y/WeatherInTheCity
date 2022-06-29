using WeatherApp.Data.Models;
using WeatherApp.Data.Services;
using WeatherApp.Business.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Polly;

namespace WeatherApp
{
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
                    services.AddHttpClient("WeatherApp")
                        .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(5, retryAttempt =>
                            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + 
                            TimeSpan.FromMilliseconds(new Random().Next(0, 1000))))
                        .AddTransientHttpErrorPolicy(builder => builder.CircuitBreakerAsync(
                            handledEventsAllowedBeforeBreaking: 3,
                            durationOfBreak: TimeSpan.FromSeconds(30)
                        ));
                });

            return hostBuilder;
        }
    }
}