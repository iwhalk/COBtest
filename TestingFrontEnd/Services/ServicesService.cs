using Shared.Models;
using TestingFrontEnd.Interfaces;

namespace TestingFrontEnd.Services
{
    public class ServicesService : IServicesService
    {
        private readonly IGenericRepository _repository;
        public ServicesService(IGenericRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<Service>> GetServicesAsync()
        {
            return await _repository.GetAsync<List<Service>>("api/Services");
        }

        public async Task<Service> PostServicesAsync(Service service)
        {
            return await _repository.PostAsync<Service>("api/Services", service);
        }
    }
}
