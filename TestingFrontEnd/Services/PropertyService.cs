using Shared.Models;
using TestingFrontEnd.Interfaces;

namespace TestingFrontEnd.Services
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
            return await _repository.PostAsync<Property>("api/Property", property);
        }
    }
}
