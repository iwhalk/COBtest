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

        public async Task<ApiResponse<List<SubElement>>> GetSubElementsAsync(int? ID_Element)
        {
            Dictionary<string, string> parameters = new();

            if (ID_Element != null && ID_Element > 0)
            {
                parameters.Add("ID_Element", ID_Element.ToString());
            }

            return await GetAsync<List<SubElement>>(path: "SubElements", parameters: parameters);
        }

        public async Task<ApiResponse<SubElement>> PostSubElementAsync(SubElement subElement)
        {
            return await PostAsync<SubElement>(subElement, path: "SubElements");
        }
    }
}
