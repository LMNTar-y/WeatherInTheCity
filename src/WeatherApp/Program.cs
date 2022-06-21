using WeatherApp.Services;
using Microsoft.Extensions.DependencyInjection;
using WeatherApp;

var host = Startup.CreateHostBuilder(args).Build();

host.Services.GetService<IMainAppService>().Run();
Console.Read();
