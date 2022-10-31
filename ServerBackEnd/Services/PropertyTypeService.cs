using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using Shared;
using Shared.Models;

namespace ApiGateway.Services
{
    public class PropertyTypeService : GenericProxy, IPropertyTypeService
    {
        public PropertyTypeService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "ReportesInmobiliaria")
        {

        }

        public async Task<ApiResponse<List<PropertyType>>> GetPropertyTypeAsync()
        {
            return await GetAsync<List<PropertyType>>(path: "propertyType");
        }

        public async Task<ApiResponse<PropertyType>> PostPropertyTypeAsync(PropertyType propertyType)
        {
            return await PostAsync<PropertyType>(propertyType, path: "propertyType");
        }
    }
}
