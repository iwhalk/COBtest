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

        public async Task<ApiResponse<SubElement>> GetSubElementAsync(int id)
        {
            Dictionary<string, string> parameters = new();

            if (id != null && id > 0)
            {
                parameters.Add("id", id.ToString());
            }

            return await GetAsync<SubElement>(id, path: "SubElement", parameters: parameters);
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
