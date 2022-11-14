namespace FrontEnd.Interfaces
{
    public interface IGenericRepository
    {
        public Task<HttpResponseMessage> GetAsync(string path);
        public Task<T>? GetAsync<T>(string path);

        public Task<HttpResponseMessage> PostAsync(string path, object value);
        public Task<T>? PostAsync<T>(string path, object value);
        public Task<T>? PostAsync<T>(string path, T value);
        public Task<T>? PutAsync<T>(string path, T value);
        public Task<T>? PutAsync<T>(string path, object value);
    }
}
