using Shared.Models;
using FrontEnd.Interfaces;

namespace FrontEnd.Services
{
    public class TenantService : ITenantService
    {
        private readonly IGenericRepository _repository;
        public TenantService(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Tenant>> GetTenantAsync()
        {
            return await _repository.GetAsync<List<Tenant>>("api/Tenant");
        }
        public async Task<Tenant> PostTenantAsync(Tenant tenant)
        {
            return await _repository.PostAsync<Tenant>("api/Tenant", tenant);
        }
    }
}
