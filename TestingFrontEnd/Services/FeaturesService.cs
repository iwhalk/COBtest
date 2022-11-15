using FrontEnd.Stores;
using SharedLibrary.Models;
﻿using FrontEnd.Interfaces;
using SharedLibrary.Models;

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
            if (_context.FeatureList == null)
            {
                var response = await _repository.GetAsync<List<Feature>>("api/Features");

                if (response != null)
                {
                    _context.FeatureList = response;
                    return _context.FeatureList;
                }
            }
            return _context.FeatureList;
        }
        public async Task<Feature> PostFeaturesAsync(Feature feature)
        {
            return await _repository.PostAsync("api/Features", feature);
        }
    }
}
