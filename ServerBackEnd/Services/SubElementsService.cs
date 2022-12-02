using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using SharedLibrary;
using SharedLibrary.Models;
using System.Xml.Linq;

namespace ApiGateway.Services
{
    public class SubElementsService : GenericProxy, ISubElementsService
    {
        public SubElementsService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {

        }

        public async Task<ApiResponse<List<SubElement>>> GetSubElementsAsync()
        {
            return await GetAsync<List<SubElement>>(path: "SubElements");
        }

        public async Task<ApiResponse<SubElement>> PostSubElementAsync(SubElement subElement)
        {
            return await PostAsync<SubElement>(subElement, path: "SubElements");
        }
    }
}
