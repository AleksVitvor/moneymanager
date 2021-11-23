namespace ExchangeRateLibrary.Service
{
    using Model;
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    public class ExchangeRateService : IExchangeRateService
    {
        public async Task<ExchangeRateGeneralInfo> GetExchangeRate(DateTime date)
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://api.exchangeratesapi.io/v1/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await httpClient.GetAsync($"/{date.ToShortDateString()}?access_key=c217147a760efc64bbb43df91ffde2c1");
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<ExchangeRateGeneralInfo>().Result;
            }
            return null;
        }
    }
}