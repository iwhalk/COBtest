﻿using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using Shared;
using Shared.Models;

namespace ApiGateway.Services
{
    public class PropertyService : GenericProxy, IPropertyService
    {
        public PropertyService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {

        }

        public async Task<ApiResponse<List<Property>>> GetPropertyAsync()
        {
            return await GetAsync<List<Property>>(path: "Properties");
        }

        public async Task<ApiResponse<Property>> PostPropertyAsync(Property property)
        {
            return await PostAsync<Property>(property, path: "Property");
        }
    }
}
