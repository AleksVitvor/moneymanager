namespace ExchangeRateLibrary.Tests;

using System.Net;

using ExchangeRateLibrary.Model;
using NUnit.Framework;
using System.Net.Http.Headers;
using System.Net.Http.Json;

public class ExchangeRateServiceTests
{
    [Test]
    public async Task TestExchangeRateApiIntegration()
    {
        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://api.exchangeratesapi.io/v1/");

        var request = new HttpRequestMessage
                          {
                              RequestUri = new Uri($"http://api.exchangeratesapi.io/v1/{DateTime.Today.ToString("yyyy-MM-dd")}?access_key=c217147a760efc64bbb43df91ffde2c1&symbols=GBP,CHF,USD,BYN,RUB,EUR"),            
                              Method = HttpMethod.Get
                          };

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        using var response = await httpClient.SendAsync(request);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
}