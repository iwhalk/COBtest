using Shared.Models;

namespace FrontEnd.Interfaces
{
    public interface IFeaturesService
    {
        Task<List<Feature>> GetFeaturesAsync();
        Task<Feature> PostFeaturesAsync(Feature feature);
    }
}
