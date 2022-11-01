using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using Shared;
using Shared.Models;

namespace ApiGateway.Services
{
    public class DescriptionService : GenericProxy, IDescriptionService
    {
        public DescriptionService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "ReportesInmobiliaria")
        {

        }

        public async Task<ApiResponse<List<Description>>> GetDescriptionAsync()
        {
            return await GetAsync<List<Description>>(path: "description");
        }

        public async Task<ApiResponse<Description>> PostDescriptionAsync(Description description)
        {
            return await PostAsync<Description>(description, path: "description");
        }
    }
}
