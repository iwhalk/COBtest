using FrontEnd.Stores;
using Shared.Models;
using FrontEnd.Interfaces;

namespace FrontEnd.Services
{
    public class PropertyTypeService : IPropertyTypeService
    {
        private readonly IGenericRepository _repository;
        private readonly ApplicationContext _context;
        public PropertyTypeService(IGenericRepository repository, ApplicationContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<List<PropertyType>> GetPropertyTypeAsync()
        {
            if (_context.PropertyType == null)
            {
                var response = await _repository.GetAsync<List<PropertyType>>("api/PropertyType");

                if (response != null)
                {
                    _context.PropertyType = response;
                    return _context.PropertyType;
                }
            }

            return _context.PropertyType;
        }

        public async Task<PropertyType> PostPropertyTypeAsync(PropertyType propertyType)
        {
            return await _repository.PostAsync<PropertyType>("api/PropertyType", propertyType);
        }
    }
}
