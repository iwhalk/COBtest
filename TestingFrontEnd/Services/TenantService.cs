using Shared.Models;
using TestingFrontEnd.Interfaces;

namespace TestingFrontEnd.Services
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
            return await _repository.GetAsync<List<Tenant>>("api/Tenant/Get");
        }
        public async Task<Tenant> PostTenantAsync(Tenant tenant)
        {
            return await _repository.PostAsync<Tenant>("api/Tenant/Post", tenant);
        }
    }
}
