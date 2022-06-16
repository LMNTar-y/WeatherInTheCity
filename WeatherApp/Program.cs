using WeatherApp.Model;
using WeatherApp.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

var services = new ServiceCollection();
ConfigureServices(services);

// create service provider
using var serviceProvider = services.BuildServiceProvider();

await serviceProvider.GetService<IMainAppService>().Run();

Console.Read();

static void ConfigureServices(IServiceCollection services)
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