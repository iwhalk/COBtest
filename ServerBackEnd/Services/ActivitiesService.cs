using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using SharedLibrary;
using SharedLibrary.Models;

namespace ApiGateway.Services
{
    public class ActivitiesService : GenericProxy, IActivitiesService
    {
        public ActivitiesService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {

        }

        public async Task<ApiResponse<Activity>> GetActivityAsync(int id)
        {
            Dictionary<string, string> parameters = new();

            if (id != null && id > 0)
            {
                parameters.Add("id", id.ToString());
            }

            return await GetAsync<Activity>(path: "Activity", parameters: parameters);
        }

        public async Task<ApiResponse<List<Activity>>> GetActivitiesAsync()
        {
            return await GetAsync<List<Activity>>(path: "Activities");
        }

        public async Task<ApiResponse<Activity>> PostActivityAsync(Activity activity)
        {
            return await PostAsync<Activity>(activity, path: "Activity");
        }
    }
}
