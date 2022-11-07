using Shared.Models;
using FrontEnd.Interfaces;

namespace FrontEnd.Services
{
    public class PropertyTypeService : IPropertyTypeService
    {
        private readonly IGenericRepository _repository;
        public PropertyTypeService(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PropertyType>> GetPropertyTypeAsync()
        {
            return await _repository.GetAsync<List<PropertyType>>("api/PropertyType");
        }

        public async Task<PropertyType> PostPropertyTypeAsync(PropertyType propertyType)
        {
            return await _repository.PostAsync<PropertyType>("api/PropertyType", propertyType);
        }
    }
}
