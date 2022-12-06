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

        public async Task<ApiResponse<List<Element>>> GetElementsAsync(int? ID_Activity)
        {
            Dictionary<string, string> parameters = new();

            if (ID_Activity != null && ID_Activity > 0)
            {
                parameters.Add("ID_Activity", ID_Activity.ToString());
            }
            return await GetAsync<List<Element>>(path: "Elements", parameters: parameters);
        }

        public async Task<ApiResponse<Element>> PostElementAsync(Element element)
        {
            return await PostAsync<Element>(element, path: "Element");
        }
    }
}
