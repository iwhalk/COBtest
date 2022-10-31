﻿using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using Shared;
using Shared.Models;

namespace ApiGateway.Services
{
    public class AreaService : GenericProxy, IAreaService
    {
        public AreaService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "ReportesInmobiliaria")
        {

        }
        public async Task<ApiResponse<List<Area>>> GetAreaAsync()
        {
            return await GetAsync<List<Area>>(path: "area");
        }

        public async Task<ApiResponse<Area>> PostAreaAsync(Area area)
        {
            return await PostAsync<Area>(area, path: "area");
        }
    }
}
