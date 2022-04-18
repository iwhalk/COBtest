using ApiGateway.Models;

namespace ApiGateway.Interfaces
{
    public interface IWeatherForecastService
    {
        Task<ApiResponse> ReadAsync(string? weatherForecast = null);
        Task<ApiResponse> ReadAsync(int id);
        Task<ApiResponse> UpdateAsync(int id, WeatherForecast weatherForecast);
        Task<ApiResponse> CreateAsync(WeatherForecast weatherForecast);
        Task<ApiResponse> RemoveAsync(int id);
    }
}
