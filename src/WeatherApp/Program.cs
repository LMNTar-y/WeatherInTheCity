using Microsoft.Extensions.DependencyInjection;
using WeatherApp;
using WeatherApp.Business.Services;

var host = Startup.CreateHostBuilder(args).Build();

host.Services.GetService<IMainAppService>().Run();
Console.Read();
