using FrontEnd.Stores;
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
            if (_context.PropertyList == null)
            {
                var response = await _repository.GetAsync<List<Property>>("api/Property");

                if (response != null)
                {
                    _context.PropertyList = response;
                    return _context.PropertyList;
                }
            }
            return _context.PropertyList;
        }
        public async Task<Property> PostPropertyAsync(Property property)
        {
            return await _repository.PostAsync("api/Property", property);
        }
    }
}
