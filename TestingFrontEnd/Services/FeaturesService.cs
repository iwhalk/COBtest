using Shared.Models;
using FrontEnd.Interfaces;

namespace FrontEnd.Services
{
    public class FeaturesService : IFeaturesService
    {
        private readonly IGenericRepository _repository;
        public FeaturesService(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Feature>> GetFeaturesAsync()
        {
            return await _repository.GetAsync<List<Feature>>("api/Features");
        }

        public async Task<Feature> PostFeaturesAsync(Feature feature)
        {
            return await _repository.PostAsync<Feature>("api/Features", feature);
        }
    }
}
