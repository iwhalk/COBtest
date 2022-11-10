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
    public class FeaturesService : IFeaturesService
    {
        private readonly IGenericRepository _repository;
        private readonly ApplicationContext _context;
        public FeaturesService(IGenericRepository repository, ApplicationContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<List<Feature>> GetFeaturesAsync()
        {
            if (_context.Feature == null)
            {
                var response = await _repository.GetAsync<List<Feature>>("api/Features");

                if (response != null)
                {
                    _context.Feature = response;
                    return _context.Feature;
                }
            }

            return _context.Feature;
        }

        public async Task<Feature> PostFeaturesAsync(Feature feature)
        {
            return await _repository.PostAsync("api/Features", feature);
        }
    }
}
