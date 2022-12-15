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
            return await GetAsync<Activity>(id, path: "Activity");
        }

        public async Task<ApiResponse<List<Activity>>> GetActivitiesAsync(int? idArea)
        {
            Dictionary<string, string> parameters = new();

            if (idArea != null && idArea > 0)
            {
                parameters.Add("idArea", idArea.ToString());
            }

            return await GetAsync<List<Activity>>(parameters, "Activities");
        }

        public async Task<ApiResponse<Activity>> PostActivityAsync(Activity activity)
        {
            return await PostAsync<Activity>(activity, path: "Activity");
        }
    }
}
