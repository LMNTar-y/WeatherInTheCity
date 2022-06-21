using WeatherApp.Data.Model;
using WeatherApp.Data.Services;
using WeatherApp.Business.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

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
                       .AddSingleton<IWeatherRecieverService, WeatherRecieverService>()
                       .AddSingleton<IMainAppService, MainAppService>();
                    services.Configure<PathToFileConfig>(context.Configuration.GetSection(nameof(PathToFileConfig)));
                    services.AddHttpClient();
                });

            return hostBuilder;
        }
    }
}