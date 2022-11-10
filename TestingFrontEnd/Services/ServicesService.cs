using FrontEnd.Interfaces;
using Shared.Models;

namespace FrontEnd.Services
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
            return await _repository.PostAsync("api/Services", service);
        }
    }
}
