using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using Shared;
using Shared.Models;

namespace ApiGateway.Services
{
    public class LessorService : GenericProxy, ILessorService
    {
        public LessorService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "ReportesInmobiliaria")
        {

        }

        public async Task<ApiResponse<List<Lessor>>> GetLessorAsync()
        {
            return await GetAsync<List<Lessor>>(path: "lessor");
        }

        public async Task<ApiResponse<Lessor>> PostLessorAsync(Lessor lessor)
        {
            return await PostAsync<Lessor>(lessor, path: "lessor"); 
        }
    }
}
