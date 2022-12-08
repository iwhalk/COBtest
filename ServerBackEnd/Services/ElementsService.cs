using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using SharedLibrary;
using SharedLibrary.Models;

namespace ApiGateway.Services
{
    public class ElementsService : GenericProxy, IElementsService
    {
        public ElementsService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {

        }

        public async Task<ApiResponse<Element>> GetElementAsync(int id)
        {
            return await GetAsync<Element>(id, path: "Element");
        }

        public async Task<ApiResponse<List<Element>>> GetElementsAsync(int? idActivity)
        {
            Dictionary<string, string> parameters = new();

            if (idActivity != null && idActivity > 0)
            {
                parameters.Add("idActivity", idActivity.ToString());
            }
            return await GetAsync<List<Element>>(path: "Elements", parameters: parameters);
        }

        public async Task<ApiResponse<Element>> PostElementAsync(Element element)
        {
            return await PostAsync<Element>(element, path: "Element");
        }
    }
}
