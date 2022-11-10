using FrontEnd.Stores;
using SharedLibrary.Models;
﻿using FrontEnd.Interfaces;
using SharedLibrary.Models;

namespace FrontEnd.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IGenericRepository _repository;
        private readonly ApplicationContext _context;
        public PropertyService(IGenericRepository repository, ApplicationContext context)
        {
            _context = context;
            _repository = repository;
        }

        public async Task<List<Property>> GetPropertyAsync()
        {
            if (_context.Property == null)
            {
                var response = await _repository.GetAsync<List<Property>>("api/Property");

                if (response != null)
                {
                    _context.Property = response;
                    return _context.Property;
                }
            }

            return _context.Property;
        }
        public async Task<Property> PostPropertyAsync(Property property)
        {
            return await _repository.PostAsync("api/Property", property);
        }
    }
}
