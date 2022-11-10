using FrontEnd.Stores;
using Shared.Models;
using FrontEnd.Interfaces;

namespace FrontEnd.Services
{
    public class TenantService : ITenantService
    {
        private readonly IGenericRepository _repository;
        private readonly ApplicationContext _context;
        public TenantService(IGenericRepository repository, ApplicationContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<List<Tenant>> GetTenantAsync()
        {
            if (_context.Tenant == null)
            {
                var response = await _repository.GetAsync<List<Tenant>>("api/Tenant");

                if (response != null)
                {
                    _context.Tenant = response;
                    return _context.Tenant;
                }
            }

            return _context.Tenant;
        }
        public async Task<Tenant> PostTenantAsync(Tenant tenant)
        {
            return await _repository.PostAsync<Tenant>("api/Tenant", tenant);
        }
    }
}
