using WeatherApp.Data.Models;
using WeatherApp.Data.Services;
using WeatherApp.Business.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace WeatherApp
{
    public class Startup
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
                    services.AddHttpClient();
                });

            return hostBuilder;
        }
    }
}