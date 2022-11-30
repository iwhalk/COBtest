using SharedLibrary.Models;
using SharedLibrary;

namespace ApiGateway.Interfaces
{
    public interface IActivitiesService
    {
        Task<ApiResponse<List<Activity>>> GetActivitiesAsync();
        Task<ApiResponse<Activity>> PostActivityAsync(Activity activity);
    }
}
