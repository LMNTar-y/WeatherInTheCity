using WeatherApp.Model;
using WeatherApp.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace WeatherApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // build config
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .AddEnvironmentVariables()
                .Build();

            services.Configure<PathToFileConfig>(configuration.GetSection("PathToFileConfig"));

            // add services:
            services.AddSingleton<IFileStorageService, FileStorageService>()
            .AddSingleton<IHttpClientService, HttpClientService>()
            .AddSingleton<IMainAppService, MainAppService>()
            .AddTransient<HttpClient>();
        }
    }
}
