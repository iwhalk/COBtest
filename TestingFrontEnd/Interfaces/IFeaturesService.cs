using Shared.Models;

namespace TestingFrontEnd.Interfaces
{
    public interface IFeaturesService
    {
        Task<List<Feature>> GetFeaturesAsync();
        Task<Feature> PostFeaturesAsync(Feature feature);
    }
}
