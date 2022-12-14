using SharedLibrary.Models;

namespace ReportesObra.Interfaces
{
    public interface IActivitiesService
    {
        Task<List<Activity>?> GetActivitiesAsync(int? idArea);
        Task<Activity?> GetActivityAsync(int id);
        Task<Activity?> CreateActivityAsync(Activity activity);
        Task<bool> UpdateActivityAsync(Activity activity);
        Task<bool> DeleteActivityAsync(int id);
    }
}
