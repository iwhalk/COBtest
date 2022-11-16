﻿using FrontEnd.Stores;
using SharedLibrary.Models;
using FrontEnd.Interfaces;
using SharedLibrary.Models;

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
            if (_context.PropertyTypeList == null)
            {
                var response = await _repository.GetAsync<List<PropertyType>>("api/PropertyType");

                if (response != null)
                {
                    _context.PropertyTypeList = response;
                    return _context.PropertyTypeList;
                }
            }

            return _context.PropertyTypeList;
        }

        public async Task<PropertyType> PostPropertyTypeAsync(PropertyType propertyType)
        {
            return await _repository.PostAsync<PropertyType>("api/PropertyType", propertyType);
        }
    }
}