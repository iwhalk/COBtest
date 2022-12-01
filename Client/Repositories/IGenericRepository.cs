namespace Client.Repositories
{
    public interface IGenericRepository
    {
        Task<T> DeleteAsync<T>(object? id, Dictionary<string, string>? parameters = null, string? path = null);
        Task<object> GetAsync(Dictionary<string, string>? parameters = null, string? path = null);
        Task<object> GetAsync(object? id, Dictionary<string, string>? parameters = null, string? path = null);
        Task<HttpResponseMessage> GetAsync(string path);
        Task<T> GetAsync<T>(Dictionary<string, string>? parameters = null, string? path = null);
        Task<T> GetAsync<T>(object? id, Dictionary<string, string>? parameters = null, string? path = null);
        Task<object> PostAsync(HttpContent? content, Dictionary<string, string>? parameters = null, string? path = null);
        Task<object> PostAsync(object? value, Dictionary<string, string>? parameters = null, string? path = null);
        Task<T> PostAsync<T>(HttpContent? content, Dictionary<string, string>? parameters = null, string? path = null);
        Task<T> PostAsync<T>(object? value, Dictionary<string, string>? parameters = null, string? path = null);
        Task<T> PostAsync<T>(T? value, Dictionary<string, string>? parameters = null, string? path = null);
        Task<object> PutAsync(object? id, HttpContent? content, Dictionary<string, string>? parameters = null, string? path = null);
        Task<object> PutAsync(object? id, object? value, Dictionary<string, string>? parameters = null, string? path = null);
        Task<T> PutAsync<T>(object? id, HttpContent? content, Dictionary<string, string>? parameters = null, string? path = null);
        Task<T> PutAsync<T>(object? id, T? value, Dictionary<string, string>? parameters = null, string? path = null);
    }
}