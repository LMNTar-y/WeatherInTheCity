using System.Text.Json;
using System.Text.RegularExpressions;
using WeatherApp.Model;
using WeatherApp.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

//using var client = new HttpClient();
//WeatherInTheCity? weather;
//string content;
//string path;

//using(StreamReader sr = new StreamReader(@"..\..\..\Configurations\appsettings.json"))
//{
//    path = new Regex(@"(?<=""storagePath"": "").*(?="")").Match(sr.ReadToEnd()).Groups[0].ToString();
//}

//try
//{
//    content = await new HttpClientService(client).GetStringAsync("Vilnius");
//    weather = JsonSerializer.Deserialize<WeatherInTheCity>(content);

//    if (weather != null)
//    {
//        using (StreamWriter sw = new StreamWriter(path, false))
//        {
//            await sw.WriteAsync(JsonSerializer.Serialize(weather));
//        }
//    }

//    Console.WriteLine("{0} TEMPERATURE: {1} °C", weather?.CityName?.ToUpper(), weather?.Temp?.CurrentTemp);
//}
//catch (Exception ex)
//{
//    Console.WriteLine(ex.Message);
//}
//finally
//{
//    client.Dispose();
//}

var host = CreateHostBuilder(args).Build();

string content = await host.Services.GetService<IMainAppService>().GetStringAsync("Vilnius");
WeatherInTheCity weather = JsonSerializer.Deserialize<WeatherInTheCity>(content);
Console.WriteLine("{0} TEMPERATURE: {1} °C", weather?.CityName?.ToUpper(), weather?.Temp?.CurrentTemp);
await host.Services.GetService<IMainAppService>().SaveAsync(weather);

Console.Read();

static IHostBuilder CreateHostBuilder(string[] args)
{
    var hostBuilder = Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((context, builder) =>
        {
            builder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@"D:\C#\projects\WeatherApp\WeatherApp\Configurations\appsettings.json");
        })
        .ConfigureServices((context, services) =>
        {
            //add your service registrations
            services.AddSingleton<IFileStorageService, FileStorageService>()
            .AddSingleton<IHttpClientService, HttpClientService>()
            .AddSingleton<IMainAppService, MainAppService>()
            .AddTransient<HttpClient>();
        });

    return hostBuilder;
}