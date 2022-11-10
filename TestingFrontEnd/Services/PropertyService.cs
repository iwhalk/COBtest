using FrontEnd.Interfaces;
using Shared.Models;

namespace FrontEnd.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IGenericRepository _repository;
        public PropertyService(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Property>> GetPropertyAsync()
        {
            return await _repository.GetAsync<List<Property>>("api/Property");
        }
        public async Task<Property> PostPropertyAsync(Property property)
        {
            return await _repository.PostAsync("api/Property", property);
        }
    }
}
