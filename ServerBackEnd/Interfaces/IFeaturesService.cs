using Shared.Models;
using Shared;

namespace ApiGateway.Interfaces
{
    public interface IFeaturesService
    {
        Task<ApiResponse<List<Feature>>> GetFeaturesAsync();
        Task<ApiResponse<Feature>> PostFeaturesAsync(Feature feature);
    }
}
