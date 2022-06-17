using WeatherApp.Services;
using Microsoft.Extensions.DependencyInjection;
using WeatherApp;

var services = new ServiceCollection();
new Startup().ConfigureServices(services);

// create service provider
using var serviceProvider = services.BuildServiceProvider();

await serviceProvider.GetService<IMainAppService>().Run();

Console.Read();

