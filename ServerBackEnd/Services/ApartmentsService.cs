﻿using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using SharedLibrary;
using SharedLibrary.Models;
using System.Diagnostics;

namespace ApiGateway.Services
{
    public class ApartmentsService : GenericProxy, IApartmentsService
    {
        public ApartmentsService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {

        }

        public async Task<ApiResponse<Apartment>> GetApartmentAsync(int id)
        {
            return await GetAsync<Apartment>(id, path: "Apartment");
        }

        public async Task<ApiResponse<List<Apartment>>> GetApartmentsAsync()
        {
            return await GetAsync<List<Apartment>>(path: "Apartments");
        }

        public async Task<ApiResponse<Apartment>> PostApartmentAsync(Apartment apartment)
        {
            return await PostAsync<Apartment>(apartment, path: "Apartment");
        }
    }
}
