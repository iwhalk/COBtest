<<<<<<< HEAD
using FrontEnd.Stores;
using Shared.Models;
using TestingFrontEnd.Interfaces;
=======
﻿using FrontEnd.Interfaces;
using Shared.Models;
>>>>>>> GatewayInmobiliaria

namespace FrontEnd.Services
{
    public class AreaService : IAreaService
    {
        private readonly IGenericRepository _repository;
        private readonly ApplicationContext _context;
        public AreaService(IGenericRepository repository, ApplicationContext context)
        {
            _repository = repository;
            _context = context; 
        }

        public async Task<List<Area>> GetAreaAsync()
        {
            if (_context.Area == null)
            {
                var response = await _repository.GetAsync<List<Area>>("api/Area");

                if (response != null)
                {
                    _context.Area = response;
                    return _context.Area;
                }
            }

            return _context.Area;
        }

        public async Task<Area> PostAreaAsync(Area area)
        {
            return await _repository.PostAsync("api/Area", area);
        }
    }
}
