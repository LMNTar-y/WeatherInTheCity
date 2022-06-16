using System.Text.Json;
using System.Text.RegularExpressions;
using WeatherApp;

using var client = new HttpClient();
WeatherInTheCity? weather;
string content;
string path;

using(StreamReader sr = new StreamReader(@"..\..\..\Configurations\StoreagePath.json"))
{
    path = new Regex(@"(?<=""storagePath"": "").*(?="")").Match(sr.ReadToEnd()).Groups[0].ToString();
}

try
{
    content = await new RequestsSender(client).GetStringAsync("Vilnius");
    weather = JsonSerializer.Deserialize<WeatherInTheCity>(content);

    if (weather != null)
    {
        using (StreamWriter sw = new StreamWriter(path, false))
        {
            await sw.WriteAsync(JsonSerializer.Serialize(weather));
        }
    }
    
    Console.WriteLine("{0} TEMPERATURE: {1} °C", weather?.CityName?.ToUpper(), weather?.Temp?.CurrentTemp);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
finally
{
    client.Dispose();
}

Console.Read();