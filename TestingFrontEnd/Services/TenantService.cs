using FrontEnd.Stores;
using SharedLibrary.Models;
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
            if (_context.TenantList == null)
            {
                var response = await _repository.GetAsync<List<Tenant>>("api/Tenant");

                if (response != null)
                {
                    _context.TenantList = response;
                    return _context.TenantList;
                }
            }

            return _context.TenantList;
        }
        public async Task<Tenant> PostTenantAsync(Tenant tenant)
        {
            return await _repository.PostAsync<Tenant>("api/Tenant", tenant);
        }
    }
}
