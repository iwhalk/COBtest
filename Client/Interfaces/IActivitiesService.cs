using SharedLibrary;
using SharedLibrary.Models;

namespace Client.Interfaces
{
    public interface IActivitiesService
    {
        Task<List<Activity>> GetActivitiesAsync();
        Task<Activity> PostActivityAsync(Activity activity);
    }
}
