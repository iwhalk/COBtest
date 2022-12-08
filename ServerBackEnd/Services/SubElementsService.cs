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
            return await GetAsync<SubElement>(id, path: "SubElement");
        }

        public async Task<ApiResponse<List<SubElement>>> GetSubElementsAsync(int? idElement)
        {
            Dictionary<string, string> parameters = new();

            if (idElement != null && idElement > 0)
            {
                parameters.Add("idElement", idElement.ToString());
            }

            return await GetAsync<List<SubElement>>(path: "SubElements", parameters: parameters);
        }

        public async Task<ApiResponse<SubElement>> PostSubElementAsync(SubElement subElement)
        {
            return await PostAsync<SubElement>(subElement, path: "SubElements");
        }
    }
}
