using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using Shared;
using Shared.Models;

namespace ApiGateway.Services
{
    public class LessorService : GenericProxy, ILessorService
    {
        public LessorService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {

        }

        public async Task<ApiResponse<List<Lessor>>> GetLessorAsync()
        {
            return await GetAsync<List<Lessor>>(path: "Lessor");
        }

        public async Task<ApiResponse<Lessor>> PostLessorAsync(Lessor lessor)
        {
            return await PostAsync<Lessor>(lessor, path: "Lessor"); 
        }
    }
}
