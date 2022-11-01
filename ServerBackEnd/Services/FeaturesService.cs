using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using Shared;
using Shared.Models;

namespace ApiGateway.Services
{
    public class FeaturesService : GenericProxy, IFeaturesService
    {
        public FeaturesService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "ReportesInmobiliaria")
        {

        }

        public async Task<ApiResponse<List<Feature>>> GetFeaturesAsync()
        {
            return await GetAsync<List<Feature>>(path: "feature");
        }

        public async Task<ApiResponse<Feature>> PostFeaturesAsync(Feature feature)
        {
            return await PostAsync<Feature>(feature, path: "feature");
        }
    }
}
