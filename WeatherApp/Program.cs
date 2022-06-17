using WeatherApp.Services;
using Microsoft.Extensions.DependencyInjection;
using WeatherApp;

#region Region First Version
//var services = new ServiceCollection();
//new Startup().ConfigureServices(services);

//// create service provider
//using var serviceProvider = services.BuildServiceProvider();

//await serviceProvider.GetService<IMainAppService>().Run();

//Console.Read();
#endregion

var host = Startup.CreateHostBuilder(args).Build();

host.Services.GetService<IMainAppService>().Run();
Console.Read();
