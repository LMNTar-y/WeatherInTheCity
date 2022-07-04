using Microsoft.Extensions.DependencyInjection;
using WeatherApp.Business;
using WeatherApp.Main;

var host = Startup.CreateHostBuilder(args).Build();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
host.Services.GetService<IReceiveAndSaveService>().Run();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
Console.Read();