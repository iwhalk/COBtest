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

        public async Task<ApiResponse> CreateAsync(WeatherForecast weatherForecast)
        {
            return await PostAsync(weatherForecast, path: "weatherforecast");
        }

        public async Task<ApiResponse> ReadAsync(string? weatherForecast = null)
        {
            return await GetAsync(path: "weatherforecast");
        }

        public async Task<ApiResponse> ReadAsync(int id)
        {
            return await GetAsync(id, path: "weatherforecast");
        }

        public async Task<ApiResponse> RemoveAsync(int id)
        {
            return await DeleteAsync(id, path: "weatherforecast");
        }

        public async Task<ApiResponse> UpdateAsync(int id, WeatherForecast weatherForecast)
        {
            return await PutAsync(id, weatherForecast, path: "weatherforecast");
        }
    }
}
