using WeatherApp.Model;
using WeatherApp.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace WeatherApp
{
    public class Startup
    {
        #region Region FirstVersion
        //public void ConfigureServices(IServiceCollection services)
        //{
        //    // build config
        //    var configuration = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json", false)
        //        .AddEnvironmentVariables()
        //        .Build();

        //    services.Configure<PathToFileConfig>(configuration.GetSection("PathToFileConfig"));

        //    // add services:
        //    services.AddSingleton<IFileStorageService, FileStorageService>()
        //    .AddSingleton<IHttpClientService, HttpClientService>()
        //    .AddSingleton<IMainAppService, MainAppService>()
        //    .AddTransient<HttpClient>();
        //}
        #endregion

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
                    .AddSingleton<IHttpClientService, HttpClientService>()
                    .AddSingleton<IMainAppService, MainAppService>()
                    .AddTransient<HttpClient>();
                    services.Configure<PathToFileConfig>(context.Configuration.GetSection("PathToFileConfig"));
                });

            return hostBuilder;
        }
    }


}
