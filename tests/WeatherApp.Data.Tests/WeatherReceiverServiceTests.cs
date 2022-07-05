using Microsoft.Extensions.Logging;
using Moq;
using WeatherApp.Data.Services;
using WeatherApp.Data.Models;
using System.Net;
using Moq.Protected;

namespace WeatherApp.Data.Tests;

public class WeatherReceiverServiceTests
{
    private readonly WeatherReceiverService _sut;
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock = new();
    private readonly Mock<ILogger<WeatherReceiverService>> _loggerMock = new();
    private readonly Mock<HttpMessageHandler>  _httpMessageHandlerMock = new();
    private HttpResponseMessage _response = new();
    private readonly HttpClient _client;

    public WeatherReceiverServiceTests()
    {
        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() => _response);

        _client = new HttpClient(_httpMessageHandlerMock.Object);
        _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(() => _client);
        _sut = new WeatherReceiverService(_httpClientFactoryMock.Object, _loggerMock.Object);
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.InternalServerError)]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.LoopDetected)]
    [InlineData(HttpStatusCode.BadGateway)]
    [InlineData(HttpStatusCode.ExpectationFailed)]
    public async Task GetWeatherAsync_ResponseStatusCodeIsNotSuccess_ThrowsExceptions(object httpStatusCode)
    {
        //arrange
        _client.BaseAddress = new Uri("https://test/");
        _response = new HttpResponseMessage((HttpStatusCode)httpStatusCode);

        //act
        //assert
        await Assert.ThrowsAnyAsync<ArgumentException>(() => _sut.GetWeatherAsync("Vilnius"));
    }

    [Fact]
    public async Task GetWeatherAsync_GetCorrectResponse_ReturnsResult()
    {
        //arrange
        var weather = new WeatherInTheCity() { CityName = "Vilnius", Temp = new TempInfo() { CurrentTemp = 16.49 } };

        _client.BaseAddress = new Uri("https://test/");

        _response = new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(@"{""main"":{ ""temp"":16.49},""name"":""Vilnius""}")
        };
        
        //act
        var result = await _sut.GetWeatherAsync("dcs");

        //assert
        Assert.NotNull(result);
        Assert.Equal(weather.CityName, result.CityName);
        Assert.Equal(weather.Temp.CurrentTemp, result.Temp?.CurrentTemp);
    }


}