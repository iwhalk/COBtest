using Shared.Models;
using TestingFrontEnd.Interfaces;

namespace TestingFrontEnd.Services
{
    public class DescriptionService : IDescriptionService
    {
        private readonly IGenericRepository _repository;
        public DescriptionService(IGenericRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<Description>> GetDescriptionAsync()
        {
            return await _repository.GetAsync<List<Description>>("api/Description");
        }

        public async Task<Description> PostDescriptionAsync(Description description)
        {
            return await _repository.PostAsync<Description>("api/Description", description);
        }
    }
}
