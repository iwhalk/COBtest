﻿<<<<<<< HEAD
using FrontEnd.Stores;
using Shared.Models;
using TestingFrontEnd.Interfaces;
=======
﻿using FrontEnd.Interfaces;
using Shared.Models;
>>>>>>> GatewayInmobiliaria

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
