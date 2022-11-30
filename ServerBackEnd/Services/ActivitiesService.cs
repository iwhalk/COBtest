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
