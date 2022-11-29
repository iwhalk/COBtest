using SharedLibrary.Models;

namespace ReportesObra.Interfaces
{
    public interface IFeaturesService
    {
        Task<List<Feature>?> GetFeaturesAsync();
        Task<Feature?> CreateFeatureAsync(Feature feature);
        Task<bool> UpdateFeatureAsync(Feature feature);
        Task<bool> DeleteFeatureAsync(int id);
    }
}