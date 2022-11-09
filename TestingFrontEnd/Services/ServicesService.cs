using Shared.Models;
using TestingFrontEnd.Interfaces;
using TestingFrontEnd.Stores;

namespace TestingFrontEnd.Services
{
    public class ServicesService : IServicesService
    {
        private readonly IGenericRepository _repository;
        private readonly ApplicationContext _context;
        public ServicesService(IGenericRepository repository, ApplicationContext context)
        {
            _repository = repository;
            _context = context; 
        }
        public async Task<List<Service>> GetServicesAsync()
        {
            if (_context.Service == null)
            {
                var response = await _repository.GetAsync<List<Service>>("api/Services");

                if (response != null)
                {
                    _context.Service = response;
                    return _context.Service;
                }
            }

            return _context.Service;
        }

        public async Task<Service> PostServicesAsync(Service service)
        {
            return await _repository.PostAsync<Service>("api/Services", service);
        }
    }
}
