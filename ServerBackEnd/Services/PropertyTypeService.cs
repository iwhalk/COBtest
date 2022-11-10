using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using SharedLibrary.Models;
using SharedLibrary;

namespace ApiGateway.Services
{
    public class PropertyTypeService : GenericProxy, IPropertyTypeService
    {
        public PropertyTypeService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {

        }

        public async Task<ApiResponse<List<PropertyType>>> GetPropertyTypeAsync()
        {
            return await GetAsync<List<PropertyType>>(path: "PropertyTypes");
        }

        public async Task<ApiResponse<PropertyType>> PostPropertyTypeAsync(PropertyType propertyType)
        {
            return await PostAsync<PropertyType>(propertyType, path: "PropertyTypes");
        }
    }
}
