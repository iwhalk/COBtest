using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using SharedLibrary;
using SharedLibrary.Data;
using SharedLibrary.Models;

namespace ApiGateway.Services
{
    public class AspNetUserService : GenericProxy, IAspNetUserService
    {
        public AspNetUserService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {

        }

        public async Task<ApiResponse<List<AspNetUser>>> GetAspNetUsersAsync()
        {
            return await GetAsync<List<AspNetUser>>(path: "AspNetUsers");
        }
    }
}
