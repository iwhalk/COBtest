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

        public async Task<ApiResponse<List<Element>>> GetElementsAsync()
        {
            return await GetAsync<List<Element>>(path: "Elements");
        }

        public async Task<ApiResponse<Element>> PostElementAsync(Element element)
        {
            return await PostAsync<Element>(element, path: "Elements");
        }
    }
}
