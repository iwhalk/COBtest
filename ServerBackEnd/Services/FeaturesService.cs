using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using SharedLibrary.Models;
using SharedLibrary;

namespace ApiGateway.Services
{
    public class FeaturesService : GenericProxy, IFeaturesService
    {
        public FeaturesService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {

        }

        public async Task<ApiResponse<List<Feature>>> GetFeaturesAsync()
        {
            return await GetAsync<List<Feature>>(path: "Features");
        }

        public async Task<ApiResponse<Feature>> PostFeaturesAsync(Feature feature)
        {
            return await PostAsync<Feature>(feature, path: "Features");
        }
    }
}
