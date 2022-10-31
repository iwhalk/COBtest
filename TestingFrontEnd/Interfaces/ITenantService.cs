﻿using Shared.Models;

namespace TestingFrontEnd.Interfaces
{
    public interface ITenantService
    {
        Task<List<Tenant>> GetTenantAsync();
        Task<Tenant> PostTenantAsync(Tenant tenant);
    }
}
