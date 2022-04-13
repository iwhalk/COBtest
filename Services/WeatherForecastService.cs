using ApiGateway.Interfaces;
using ApiGateway.Models;
using ApiGateway.Proxies;

namespace ApiGateway.Services
{
    public class WeatherForecastService : GenericProxy<WeatherForecast>, IWeatherForecastService
    {
        public WeatherForecastService(IHttpContextAccessor httpContextAccessor,
                                      HttpClient httpClient,
                                      IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClient, httpClientFactory)
        {
        }

        public Task<WeatherForecast> CreateAsync(WeatherForecast weatherForecast)
        {
            throw new NotImplementedException();
        }

        public async Task<byte[]> ReadAsync(string? weatherForecast = null)
        {
            return await GetStreamAsync(path: "weatherforecast");

        }

        public Task<WeatherForecast> ReadAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(int id, WeatherForecast weatherForecast)
        {
            throw new NotImplementedException();
        }
    }
}
