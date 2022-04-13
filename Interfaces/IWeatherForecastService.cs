using ApiGateway.Models;

namespace ApiGateway.Interfaces
{
    public interface IWeatherForecastService
    {
        Task<byte[]> ReadAsync(string? weatherForecast = null);
        Task<WeatherForecast> ReadAsync(int id);
        Task<bool> UpdateAsync(int id, WeatherForecast weatherForecast);
        Task<WeatherForecast> CreateAsync(WeatherForecast weatherForecast);
        Task<bool> RemoveAsync(int id);
    }
}
